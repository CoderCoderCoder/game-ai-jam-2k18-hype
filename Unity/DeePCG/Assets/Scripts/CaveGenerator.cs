using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CaveGenerator : MonoBehaviour {

    public Tilemap tilemap;
    public Tile blankTile;
    public Tile rockTile;
    public Tile floorTile;
    public Tile floorRightTile;
    public Tile floorLeftTile;
    public Tile floorRightCorner;
    public Tile floorLeftCorner;
    public Tile ceilingTile;
    public Tile ceilingLeftTile;
    public Tile ceilingRightTile;
    public Tile ceilingRightCorner;
    public Tile ceilingLeftCorner;
    public Tile wallLeftTile;
    public Tile wallRightTile;

    public int rows = 32;
    public int columns = 32;
    public int buffer = 20;

    public int iterations = 6;
    public float initProb = 0.5f;

    public int rockThreshold = 5;

    private int[,] grid;
    private int[,] gridCopy;

    private List<Vector3Int> floorCoordinates;
    private List<Vector3Int> waterCoordinates;
    public Dictionary<Vector3Int, GameObject> flora;
       
    void Start()
    {
        grid = new int[rows, columns];
        gridCopy = new int[rows, columns];

        floorCoordinates = new List<Vector3Int>();
        waterCoordinates = new List<Vector3Int>();
        flora = new Dictionary<Vector3Int, GameObject>();

        Generate();
    }

    void Generate()
    {
        PopulateInitially();

        for (int i = 0; i < iterations; ++i)
        {
            Iterate();
        }

        PopulateTileset();
    }

    void PopulateInitially()
    {
        //Initial population of the grid with rock cells.
        for (int i = 0; i < rows; ++i)
        {
            for (int j = 0; j < columns; ++j)
            {
                grid[i, j] = 0;

                if (Random.Range(0.0f, 1.0f) < initProb)
                    grid[i, j] = 1;
            }
        }
    }
	
    void Iterate()
    {
        for(int i = 0; i < rows; ++i)
        {
            for(int j = 0; j < columns; ++j)
            {
                gridCopy[i, j] = grid[i, j];
            }
        }

        for (int i = 1; i < rows - 1; ++i)
        {
            for (int j = 1; j < columns - 1; ++j)
            {
                int rockCount = 0;

                for(int m = i - 1; m <= i + 1; ++m)
                {
                    for(int n = j - 1; n <= j + 1; ++n)
                    {
                        if (m == i && n == j)
                            continue;

                        if (grid[m, n] == 1)
                            ++rockCount;
                    }
                }

                if (rockCount >= rockThreshold)
                    gridCopy[i, j] = 1;
                else
                    gridCopy[i, j] = 0;                   
            }
        }

        for(int i = 0; i < rows; ++i)
        {
            for(int j = 0; j < columns; ++j)
            {
                grid[i, j] = gridCopy[i, j];
            }
        }
    }

    public Vector3 GetGridWorldspace(int i, int j)
    {
        return tilemap.GetCellCenterWorld(new Vector3Int(i, j, 0));
    }

    public int WaterSize()
    {
        return waterCoordinates.Count;
    }

    public int FloorSize()
    {
        return floorCoordinates.Count;
    }

    public void TryDestroyTile(Vector3 pos)
    {
        Vector3Int tileCoords = tilemap.WorldToCell(pos);

        if (tileCoords.x < buffer || tileCoords.x > (rows - buffer) || tileCoords.y < buffer || tileCoords.y > (columns - buffer))
            return;

        if(flora.ContainsKey(tileCoords))
        {
            Destroy(flora[tileCoords]);
            flora.Remove(tileCoords);
        }

        tilemap.SetTile(tileCoords, null);
    }

    public Vector3 GetWaterCoords(int index)
    {
        return tilemap.GetCellCenterWorld(waterCoordinates[index]);
    }

    public Vector3 GetFloorCoords(int index)
    {
        return tilemap.GetCellCenterWorld(floorCoordinates[index]) + tilemap.cellSize.y * 0.4f * Vector3.up;
    }

    public Vector3Int GetTileCoords(Vector3 pos)
    {
        return tilemap.WorldToCell(pos);
    }

	void PopulateTileset()
    {
        for(int i = 0; i < rows; ++i)
        {
            for(int j = 0; j < buffer; ++j)
            {
                grid[i, j] = grid[i, columns - j - 1] = 1;
            }
        }

        for(int j = 0; j < columns; ++j)
        {
            for(int i = 0; i < buffer; ++i)
            {
                grid[i, j] = grid[rows - i - 1, j] = 1;
            }
        }

        tilemap.ClearAllTiles();
        floorCoordinates.Clear();
        flora.Clear();

        Vector3Int playerOrigin = tilemap.WorldToCell(Vector3.zero);
        for(int i = -1; i < 2; ++i)
        {
            for(int j = -1; j < 2; ++j)
            {
                grid[playerOrigin.x + i, playerOrigin.y + j] = 0;
            }
        }

        int[,] surround = new int[3, 3];

        for(int i = 0; i < rows; ++i)
        {
            for(int j = 0; j < columns; ++j)
            {
                Vector3Int pos = new Vector3Int(i, j, 0);

                if (grid[i, j] == 1)
                {
                    for (int m = i - 1, x = 0; m <= i + 1; ++m, ++x)
                    {
                        for (int n = j - 1, y = 0; n <= j + 1; ++n, ++y)
                        {
                            if (m < 0 || m >= rows || n < 0 || n >= columns)
                            {
                                surround[x, y] = 1;
                                continue;
                            }

                            surround[x, y] = grid[m, n];
                        }
                    }

                    bool rightFree = surround[2, 1] == 0;
                    bool leftFree = surround[0, 1] == 0;
                    
                    if (j < columns - 1 && grid[i, j + 1] == 0)
                    {
                        //We're a floor.
                        if (rightFree)
                        {
                            if (surround[2, 2] == 0)
                                tilemap.SetTile(pos, floorRightCorner);
                            else
                                tilemap.SetTile(pos, floorRightTile);
                        }
                        else if (leftFree)
                        {
                            if (surround[0, 0] == 0)
                                tilemap.SetTile(pos, floorLeftCorner);
                            else
                                tilemap.SetTile(pos, floorLeftTile);
                        }
                        else
                        {
                            tilemap.SetTile(pos, floorTile);
                        }

                        floorCoordinates.Add(pos);
                    }
                    else if (j > 0 && grid[i, j - 1] == 0)
                    {
                        //We're a ceiling.
                        if (rightFree)
                            tilemap.SetTile(pos, ceilingRightTile);
                        else if (leftFree)
                            tilemap.SetTile(pos, ceilingLeftTile);
                        else
                            tilemap.SetTile(pos, ceilingTile);
                    }
                    else if (rightFree)
                        tilemap.SetTile(pos, wallRightTile);
                    else if (leftFree)
                        tilemap.SetTile(pos, wallLeftTile);
                    else
                    {
                        bool haveRock = Random.Range(0.0f, 1.0f) < 0.1f;
                        //We're a doctor, Jim.
                        tilemap.SetTile(pos, haveRock ? rockTile : blankTile);
                    }

                }
                else
                    waterCoordinates.Add(pos);
            }
        }
    }
}
