using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrammarGenerator : MonoBehaviour {

    public CaveGenerator cave;

    public List<GameObject> flora;
    public List<GameObject> enemies;
    public List<GameObject> pickups;

    public enum ObjType
    {
        FLORA = 0,
        ENEMY = 100,
        PICKUP = 200,    
        BLANK = 300,
        BLANK_WATER = 400,
        TERMINAL = 500
    };

    [System.Serializable]
    public class GrammarRule
    {
        public string key;
        public List<string> transforms;
        public List<float> probabilities;
    }

    public List<GrammarRule> rules;
    private Dictionary<string, GrammarRule> ruleDict;

    private Dictionary<char, ObjType> charToObj;
    private Dictionary<ObjType, char> objToChar;

    private List<GameObject> worldContents;

    public string floorAxiom;
    public int floorIterations = 4;
    public int floorSkipMin = 1;
    public int floorSkipMax = 6;
    public string waterAxiom;
    public int waterSkipMin = 6;
    public int waterSkipMax = 20;
    public int waterIterations = 4;

    private string floorString;
    private string waterString;

	void Start ()
    {
        charToObj = new Dictionary<char, ObjType>();
        charToObj.Add('f', ObjType.FLORA);
        charToObj.Add('e', ObjType.ENEMY);
        charToObj.Add('p', ObjType.PICKUP);
        charToObj.Add('b', ObjType.BLANK);
        charToObj.Add('w', ObjType.BLANK_WATER);
        charToObj.Add('x', ObjType.TERMINAL);

        objToChar = new Dictionary<ObjType, char>();

        foreach(KeyValuePair<char, ObjType> kv in charToObj)
        {
            objToChar.Add(kv.Value, kv.Key);
        }

        ruleDict = new Dictionary<string, GrammarRule>();
        for(int i = 0; i < rules.Count; ++i)
        {
            ruleDict.Add(rules[i].key, rules[i]);
        }

        worldContents = new List<GameObject>();

        Generate();
	}

    public void Generate()
    {
        for(int i = 0; i < worldContents.Count; ++i)
        {
            Destroy(worldContents[i]);
        }
        worldContents.Clear();

        waterString = waterAxiom;

        for (int i = 0; i < waterIterations; ++i)
        {
            waterString = Iterate(waterString);
        }

        floorString = floorAxiom;

        for (int i = 0; i < floorIterations; ++i)
        {
            floorString = Iterate(floorString);
        }

        print("Water: " + waterString);
        print("Floor: " + floorString);

        Populate();
    }

    string Iterate(string axiom)
    {
        string result = "";

        for(int i = 0; i < axiom.Length; ++i)
        {
            GrammarRule rule = ruleDict[axiom[i].ToString()];
            float prob = Random.Range(0.0f, 1.0f);

            int index = 0;

            for(int j = 0; j < rule.probabilities.Count; ++j)
            {
                if (prob <= rule.probabilities[j])
                {
                    index = j;
                    break;
                }                
            }

            result += rule.transforms[index];
        }

        return result;
    }

    void Populate()
    {
        worldContents.Clear();

        float enemyDivision = 1.0f / enemies.Count;
        float floraDivision = 1.0f / flora.Count;
        float pickupDivision = 1.0f / pickups.Count;

        int numWaterTiles = cave.WaterSize();
        int numFloorTiles = cave.FloorSize();
        int tilesProcessed = 0;
        int tileSkip = 0;

        int numPickups = 0;

        for (int i = 0, c = 0; i < numWaterTiles && c < waterString.Length; i += tileSkip, ++c, tilesProcessed += tileSkip)
        {
            char tile = waterString[c];
            float prob = Random.Range(0.0f, 1.0f);
            int index = 0;

            switch(charToObj[tile])
            {
                case ObjType.ENEMY:

                    index = Mathf.Min((int)(prob / enemyDivision), enemies.Count - 1);
                    GameObject newEnemy = Instantiate(enemies[index]) as GameObject;
                    newEnemy.transform.position = cave.GetWaterCoords(i);
                    worldContents.Add(newEnemy);

                    break;

                case ObjType.PICKUP:

                    ++numPickups;
                    index = Mathf.Min((int)(prob / pickupDivision), pickups.Count - 1);
                    GameObject newPickup = Instantiate(pickups[index]) as GameObject;
                    newPickup.transform.position = cave.GetWaterCoords(i);
                    worldContents.Add(newPickup);

                    break;
            }

            tileSkip = Random.Range(waterSkipMin, waterSkipMax + 1);
        }

        print("Water Tile Spec Surplus: " + (tilesProcessed - numWaterTiles));

        FindObjectOfType<PlayerController>().SetNumTreasures(numPickups);

        tilesProcessed = 0;
        for (int i = 0, c = 0; i < numFloorTiles && c < floorString.Length; i += tileSkip, ++c, tilesProcessed += tileSkip)
        {
            char tile = floorString[c];
            float prob = Random.Range(0.0f, 1.0f);
            int index = 0;

            switch (charToObj[tile])
            {
                case ObjType.FLORA:

                    index = Mathf.Min((int)(prob / floraDivision), flora.Count - 1);
                    GameObject newFlora = Instantiate(flora[index]) as GameObject;
                    newFlora.transform.position = cave.GetFloorCoords(i);

                    cave.flora.Add(cave.GetTileCoords(newFlora.transform.position), newFlora);
                    worldContents.Add(newFlora);

                    break;
            }

            tileSkip = Random.Range(floorSkipMin, floorSkipMax + 1);
        }

        print("Floor Tile Spec Surplus: " + (tilesProcessed - numFloorTiles));
    }
}
