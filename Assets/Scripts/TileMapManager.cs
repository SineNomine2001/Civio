using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapManager : MonoBehaviour
{
    public GridLayout grid;
    public Tilemap terrainMap;
    public Tilemap forestMap;
    public Tilemap fogOfWarMap;
    public Res[,] resMultiplierMatrix;

    protected static TileMapManager s_Instance;
    public static TileMapManager Instance
    {
        get
        {
            if (s_Instance != null)
                return s_Instance;
            s_Instance = FindObjectOfType<TileMapManager>();

            return s_Instance;
        }
    }

    void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        s_Instance = this;
        GenerateResMultiplier();
    }

    void GenerateResMultiplier()
    {
        Vector2Int mapSize = TileAutomata.GetMapSize();
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                Vector3Int coord = new Vector3Int(x, y, 0);
                resMultiplierMatrix[x, y] = ((MapRuleTile)terrainMap.GetTile(coord)).resMultiplier;
            }
        }
    }

    public void Clear()
    {
        terrainMap.ClearAllTiles();
        forestMap.ClearAllTiles();
        fogOfWarMap.ClearAllTiles();
    }

    public Vector3 CoordToWorld(Vector2Int coord)
    {
        return grid.CellToWorld((Vector3Int)coord);
    }

    public Building GetBuildingAtCoord(Vector2Int coord)
    {
        Vector3 posWorld = CoordToWorld(coord) + new Vector3(0.5f, 0.5f);
        RaycastHit2D hit = Physics2D.Raycast(posWorld, Vector3.zero);
        if (hit.collider != null)
            return hit.collider.GetComponent<Building>();
        return null;
    }

    public HashSet<Building> GetBuildingsInTiles(HashSet<Vector2Int> tiles)
    {
        HashSet<Building> buildings = new HashSet<Building>();
        foreach (Vector2Int tile in tiles)
        {
            Building building = GetBuildingAtCoord(tile);
            if (building != null)
                buildings.Add(building);
        }
        return buildings;
    }

    // Separated for placement of derelict buildings during map generation, which does not belong to any player.
    public PlaceBuildingResult TestBuildingPlacement(BuildingData data)
    {
        foreach (Vector2Int tile in data.occupiedTiles)
        {
            TileBase F = forestMap.GetTile((Vector3Int)tile);
            bool isForestOK = data.stats.isForest || F == null;
            if (!isForestOK)
                return PlaceBuildingResult.ForestMismatch;

            Building B = GetBuildingAtCoord(tile);
            bool isBuildingOK = B == null;
            if (!isBuildingOK)
                return PlaceBuildingResult.BlockedByOtherBuildings;
        }

        return PlaceBuildingResult.Success;
    }
}
