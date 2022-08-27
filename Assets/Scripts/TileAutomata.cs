using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TileAutomata
{
    #region General
    static string mapSeed;
    static int randSeed;
    static Vector2Int mapSize;
    /// <summary>
    /// Prevent infinite loops
    /// </summary>
    const int MAXITERATION = 50;
    #endregion
    #region Altitude
    static Vector2Int offset;
    static int smoothness;
    static float waterBound;
    static float mountainBound;
    static float glacierBound;
    static float riverSpawnBound;
    #endregion
    #region Forest Distribution
    static Vector2Int poffset;
    static int psmoothness;
    static int epoch;
    #endregion
    #region Biome setting by [initChance, birthLimit, deathLimit, iterations]
    static int[] plainSetting;
    static int[] savannaSetting;
    static int[] thinForestSetting;
    static int[] forestSetting;
    static int[] thickForestSetting;
    static float plain_savannaBound;
    static float savanna_thinBound;
    static float thin_forestBound;
    static float forest_thickBound;
    static float mangroveLimit;
    static float alpineLimit;
    #endregion
    #region Rivers
    static int riverNumber;
    static float riverNoiseFactor;
    #endregion
    #region Map Matrices
    static float[,] heightMatrix;
    static float[,] precipitationMatrix;
    static int[,][] biomeMatrix;
    static int[,] forestMatrix;
    #endregion

    #region Map Generation
    static void GenerateTerrain()
    {
        heightMatrix = Algorithms.Perlin(mapSize, offset, smoothness);

        for (int x = -100; x < mapSize.x + 100; x++)
        {
            for (int y = -50; y < mapSize.y + 50; y++)
            {
                Vector3Int coord = new Vector3Int(x, y, 0);

                if (x >= 0 && x < mapSize.x && y >= 0 && y < mapSize.y)
                {
                    float h = heightMatrix[x, y];

                    if (h <= waterBound)
                        TileMapManager.Instance.terrainMap.SetTile(coord, StaticData.WaterTile);
                    else if (h >= mountainBound && h < glacierBound)
                        TileMapManager.Instance.terrainMap.SetTile(coord, StaticData.RockyTile);
                    else if (h >= glacierBound)
                        TileMapManager.Instance.terrainMap.SetTile(coord, StaticData.GlacierTile);
                    else
                        TileMapManager.Instance.terrainMap.SetTile(coord, StaticData.GrassTile);
                }
                else
                {
                    TileMapManager.Instance.fogOfWarMap.SetTile(coord, StaticData.FogOfWarTile);
                }
            }
        }
    }

    static void GenerateBiomeMatrix()
    {
        precipitationMatrix = Algorithms.Perlin(mapSize, poffset, psmoothness);
        biomeMatrix = new int[mapSize.x, mapSize.y][];

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                float ppt = precipitationMatrix[x, y];

                if (ppt <= plain_savannaBound)
                    biomeMatrix[x, y] = plainSetting;
                else if (ppt > plain_savannaBound && ppt <= savanna_thinBound)
                    biomeMatrix[x, y] = savannaSetting;
                else if (ppt > savanna_thinBound && ppt <= thin_forestBound)
                    biomeMatrix[x, y] = thinForestSetting;
                else if (ppt > thin_forestBound && ppt <= forest_thickBound)
                    biomeMatrix[x, y] = forestSetting;
                else
                    biomeMatrix[x, y] = thickForestSetting;
            }
        }

    }

    static void InitForest()
    {
        forestMatrix = new int[mapSize.x, mapSize.y];

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                forestMatrix[x, y] = Random.Range(1, 101) < biomeMatrix[x, y][0] ? 1 : 0;
            }
        }
    }

    static void GenerateForest()
    {
        InitForest();
        forestMatrix = Algorithms.Cellular(forestMatrix, mapSize, biomeMatrix, epoch);

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                Vector3Int coord = new Vector3Int(x, y, 0);
                float h = heightMatrix[x, y];

                if (forestMatrix[x, y] == 1)
                {
                    if (TileMapManager.Instance.terrainMap.GetTile(coord) == StaticData.GrassTile)
                        TileMapManager.Instance.forestMap.SetTile(coord, StaticData.ForestTile);
                    else if (h <= waterBound && Random.Range(mangroveLimit, 0.5f) <= h)
                        TileMapManager.Instance.forestMap.SetTile(coord, StaticData.MangroveTile);
                    else if (h >= mountainBound && Random.Range(0.5f, alpineLimit) >= h)
                        TileMapManager.Instance.forestMap.SetTile(coord, StaticData.AlpineTile);
                }
            }
        }
    }

    static List<Vector3Int> FindAllPossibleSpawn()
    {
        List<Vector3Int> coord = new List<Vector3Int>();

        // Find all tiles capable of spawning river
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                float h = heightMatrix[x, y];

                if (h >= riverSpawnBound)
                {
                    coord.Add(new Vector3Int(x, y, 0));
                }
            }
        }
        return coord;
    }

    static void GenerateRiver()
    {
        List<Vector3Int> possibleSpawnCoord = FindAllPossibleSpawn();
        // Randomly spawn n rivers, where n == RIVERNUM
        for (int i = 0; i < riverNumber; i++)
        {
            // Pick a spawn
            int rnd = Random.Range(0, possibleSpawnCoord.Count);
            // Keep decending until we reach water level
            Vector3Int nextCoord = possibleSpawnCoord[rnd];
            int currentIteration = 0;
            while (
                // It is within the map
                IsInMapRange((Vector2Int)nextCoord) &&
                // It is above water level
                heightMatrix[nextCoord.x, nextCoord.y] > waterBound &&
                currentIteration < MAXITERATION
                )
            {
                // Set as water
                Vector3Int pos = new Vector3Int(nextCoord.x, nextCoord.y, 0);
                TileMapManager.Instance.terrainMap.SetTile(pos, StaticData.WaterTile);

                // Examine 8 neighbours
                List<Vector3Int> neighbours = new List<Vector3Int>();

                for (int m = -1; m <= 1; m++)
                {
                    neighbours.Add(new Vector3Int(nextCoord.x + m, nextCoord.y, 0));
                }
                for (int n = -1; n <= 1; n++)
                {
                    neighbours.Add(new Vector3Int(nextCoord.x, nextCoord.y + n, 0));
                }

                float lowestAlt = heightMatrix[nextCoord.x, nextCoord.y];
                int lowestIndex = 5;

                for (int j = 0; j < neighbours.Count; j++)
                {
                    Vector3Int currentCoord = neighbours[j];
                    // Make sure it is within the bounds of the map
                    if (currentCoord.x < heightMatrix.GetLength(0) && currentCoord.x >= 0)
                    {
                        if (currentCoord.y < heightMatrix.GetLength(1) && currentCoord.y >= 0)
                        {
                            float currentAlt = heightMatrix[currentCoord.x, currentCoord.y];
                            if (currentAlt < lowestAlt)
                            {
                                lowestAlt = currentAlt;
                                lowestIndex = j;
                            }
                        }
                        else
                        {
                            currentIteration = MAXITERATION;
                        }
                    }
                    else
                    {
                        currentIteration = MAXITERATION;
                    }
                }

                // next tile has the lowest alt
                nextCoord = neighbours[lowestIndex];

                // At this step we add in noise to bend the river
                float noiseVal = Random.value;
                if (noiseVal >= 1 - riverNoiseFactor)
                {
                    TileMapManager.Instance.terrainMap.SetTile(nextCoord, StaticData.WaterTile);
                    // Randomly bend the river to some direction
                    Vector3Int distortion;
                    if (Random.Range(0f,1f) < 0.5f)
                    {
                        distortion = new Vector3Int(0, Random.Range(-1, 1), 0);
                    }
                    else
                    {
                        distortion = new Vector3Int(Random.Range(-1, 1), 0, 0);
                    }
                    nextCoord += distortion;
                }
                currentIteration++;
            }
        }
    }

    static void InitPlayers()
    {
        Vector2Int playerPos = new Vector2Int(200, 70);
        GameUI.Instance.player.Init(playerPos);
        CameraMover.Instance.MoveCamera(playerPos);
    }
    #endregion
    #region Public Functions
    public static Vector2Int GetMapSize()
    {
        return mapSize;
    }

    public static bool IsInMapRange(Vector2Int coord)
    {
        return coord.x >= 0 && coord.x < mapSize.x && coord.y >= 0 && coord.y < mapSize.y;
    }

    public static void GenerateMap()
    {
        TileMapManager.Instance.Clear();
        Random.InitState(randSeed);
        offset = new Vector2Int(Random.Range(0, 1000), Random.Range(0, 1000));
        poffset = new Vector2Int(Random.Range(0, 1000), Random.Range(0, 1000));

        GenerateTerrain();
        GenerateRiver();
        GenerateBiomeMatrix();
        GenerateForest();
        InitPlayers();
    }

    public static void InitParas()
    {
        randSeed = 0;
        mapSize = new Vector2Int(240, 140);

        smoothness = 5;
        waterBound = 0.32f;
        mountainBound = 0.64f;
        glacierBound = 0.85f;
        riverSpawnBound = 0.70f;

        psmoothness = 2;
        epoch = 6;

        plainSetting = new int[] { 10, 3, 2 };
        savannaSetting = new int[] { 20, 3, 2 };
        thinForestSetting = new int[] { 25, 3, 2 };
        forestSetting = new int[] { 25, 3, 1 };
        thickForestSetting = new int[] { 15, 2, 1 };
        plain_savannaBound = 0.45f;
        savanna_thinBound = 0.5f;
        thin_forestBound = 0.65f;
        forest_thickBound = 0.7f;
        mangroveLimit = 0.25f;
        alpineLimit = 0.75f;

        riverNumber = 10;
        riverNoiseFactor = 0.9f;
    }

    public static void GetSeed()
    {
        mapSeed = randSeed + "#" + mapSize.x + "#" + mapSize.y + "#" + smoothness + "#";
    }// Do this

    public static void SetSeed()
    {

    }// Do this
    #endregion
}
