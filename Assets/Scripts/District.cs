using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class District : MonoBehaviour
{
    public GameObject buildingPrefab;
    #region Properties
    public Player OwnerPlayer { get; private set; }
    public string Name { get; private set; }
    public DistrictType Type { get; private set; }
    public HashSet<Building> Buildings { get; private set; } = new HashSet<Building>();
    public HashSet<Building> Centers { get; private set; } = new HashSet<Building>();
    public HashSet<Vector2Int> OccupiedTiles { get; private set; } = new HashSet<Vector2Int>();
    public HashSet<Vector2Int> AdjacentTiles { get; private set; } = new HashSet<Vector2Int>();
    public bool IsOperational { get; private set; }
    public Res DResource { get; private set; }
    public int Capacity { get; private set; }
    public Dictionary<UnitStats, int> TrainableUnits { get; private set; }

    bool isInitialized = false;
    #endregion
    #region Private Functions
    void UpdateStatus()
    {
        // Check if the district has an operating center
        if (Centers.Count == 0)
            IsOperational = false;
        else
        {
            IsOperational = false;
            foreach (Building center in Centers)
            {
                if (center.IsOperational)
                    IsOperational = true;
            }
        }

        // Update variables if it is operating
        DResource = Res.zero;
        Capacity = 0;
        OccupiedTiles = new HashSet<Vector2Int>();
        AdjacentTiles = new HashSet<Vector2Int>();
        if (IsOperational)
        {
            foreach (Building building in Buildings)
            {
                if (building.IsOperational)
                {
                    DResource += building.DResource;
                    Capacity += building.Capacity;
                    OccupiedTiles.UnionWith(building.Data.occupiedTiles);
                    AdjacentTiles.UnionWith(building.Data.adjacentTiles);
                }
            }
            AdjacentTiles.ExceptWith(OccupiedTiles);
        }
    }

    void AddBuilding(Building building)
    {
        Buildings.Add(building);
        if (building.Data.key % 100 == 0)
            Centers.Add(building);
        UpdateStatus();
    }
    #endregion
    #region Public Functions
    public District Init(string name, BuildingData centerData)
    {
        if (!isInitialized)
        {
            OwnerPlayer = transform.parent.GetComponent<Player>();
            Name = name;
            Type = (DistrictType)(centerData.key / 100);
            AdjacentTiles.Add(centerData.bottomLeftCorner);
            PlaceBuilding(centerData, out _);

            IsOperational = true;
            isInitialized = true;
        }

        return this;
    }

    public PlaceBuildingResult PlaceBuilding(BuildingData data, out Building newBuilding)
    {
        newBuilding = null;
        PlaceBuildingResult result = TilemapManager.Instance.TestBuildingPlacement(data);
        if (result == PlaceBuildingResult.Success)
        {
            bool isTypeOK = (int)Type == data.key / 100;
            if (!isTypeOK)
                return PlaceBuildingResult.BuildingDistrictTypeMismatch;

            HashSet<Vector2Int> o = data.occupiedTiles;
            o.IntersectWith(AdjacentTiles);
            bool isAdjacent = o.Count != 0;
            if (!isAdjacent)
                return PlaceBuildingResult.NotAdjacentToDistrict;

            // Cost is tested last to avoid deduction of resources before other criterias are tested
            bool isAffordable = OwnerPlayer.ChangeResource(-StaticData.BDict[data.key].cost);
            if (!isAffordable)
                return PlaceBuildingResult.NotAffordable;

            GameObject go = Instantiate(buildingPrefab, transform);
            newBuilding = go.GetComponent<Building>().Init(data);
            AddBuilding(newBuilding);
        }

        return result;
    }

    public void DeleteBuilding(Building building)
    {
        Buildings.Remove(building);
        if (Buildings.Count == 0)
            OwnerPlayer.DeleteDistrict(this);
        if (building.Data.key % 100 == 0)
            Centers.Remove(building);
        UpdateStatus();

        Destroy(building.gameObject);
    }
    #endregion
}
