using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CaveGenerator : MonoBehaviour {

    public Tilemap tilemap;
    public Tile rockTile;
    public Tile floorTile;
    public Tile ceilingTile;

    public int rows = 32;
    public int columns = 32;

    public int iterations = 6;
    public float initProb = 0.5f;

    public int rockThreshold = 5;

    private int[,] grid;
    private int[,] gridCopy;

    void Start()
    {
        grid = new int[rows, columns];
        gridCopy = new int[rows, columns];

        PopulateInitially();

        for(int i = 0; i < iterations; ++i)
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

	void PopulateTileset()
    {
        tilemap.ClearAllTiles();

        for(int i = 0; i < rows; ++i)
        {
            for(int j = 0; j < columns; ++j)
            {
                if (grid[i, j] == 1)
                {
                    Vector3Int pos = new Vector3Int(i, j, 0);

                    if (j < columns - 1 && grid[i, j + 1] == 0)
                    {
                        //We're a floor.
                        tilemap.SetTile(pos, floorTile);
                    }
                    else if (j > 0 && grid[i, j - 1] == 0)
                    {
                        //We're a ceiling.
                        tilemap.SetTile(pos, ceilingTile);
                    }               
                    else //We're a doctor, Jim.
                        tilemap.SetTile(pos, rockTile);
                }                    
            }
        }
    }
}
