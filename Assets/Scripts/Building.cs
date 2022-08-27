using System.Collections.Generic;
using UnityEngine;
using Gamekit2D;

[RequireComponent(typeof(Damageable))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class Building : MonoBehaviour
{
    #region Properties
    public District OwnerDistrict { get; private set; }
    public BuildingData Data { get; private set; }
    public bool IsOperational { get; private set; }

    public Damageable HP { get; private set; }

    Res dresource;
    public Res DResource { get { return Data.stats.dresource + Calc_drAdj(); } private set { dresource = value; } }

    int capacity;
    public int Capacity { get { return Data.stats.capacity + Calc_capAdj(); } private set { capacity = value; } }

    bool isInitialized = false;
    #endregion
    #region Public Functions
    public Building Init(BuildingData data)
    {
        if (!isInitialized)
        {
            OwnerDistrict = transform.parent.GetComponent<District>();
            Data = data;
            IsOperational = true;
            HP = GetComponent<Damageable>();
            HP.startingHealth = data.stats.hp;
            DResource = data.stats.dresource;
            Capacity = data.stats.capacity;

            transform.position = TileMapManager.Instance.CoordToWorld(data.bottomLeftCorner);
            GetComponent<SpriteRenderer>().sprite = StaticData.GetBuildingSprite(data.key);
            BoxCollider2D collider = GetComponent<BoxCollider2D>();
            collider.size = data.stats.size;
            collider.offset = (Vector2)data.stats.size / 2;

            isInitialized = true;
        }

        return this;
    }

    public void OnMouseDown()
    {
        // Show detail of building
        if (!PlaceBuildingUI.Instance.GetComponent<Canvas>().enabled)
        {
            CameraMover.Instance.MoveCamera(Data.centerPos);
            CreateBuilding_UnitUI.Instance.Show(OwnerDistrict, true);
            DistrictInfoUI.Instance.Show(OwnerDistrict);
            BuildingInfoUI.Instance.Show(this);
        }
    }

    Res Calc_drAdj()
    {
        Res dr = Res.zero;
        Dictionary<int, Res> drAdj = Data.stats.drAdj;
        if (drAdj != null)
        {
            HashSet<Building> adjacentBuildings = TileMapManager.Instance.GetBuildingsInTiles(Data.adjacentTiles);
            Debug.Log(adjacentBuildings.Count);
            foreach (Building adjBuilding in adjacentBuildings)
            {
                int key = adjBuilding.Data.key;
                if (drAdj.ContainsKey(key))
                    dr += drAdj[key];
            }
        }
        return dr;
    }

    int Calc_capAdj()
    {
        int cap = 0;
        Dictionary<int, int> capAdj = Data.stats.capAdj;
        if (capAdj != null)
        {
            HashSet<Building> adjacentBuildings = TileMapManager.Instance.GetBuildingsInTiles(Data.adjacentTiles);
            Debug.Log(adjacentBuildings.Count);
            foreach (Building adjBuilding in adjacentBuildings)
            {
                int key = adjBuilding.Data.key;
                if (capAdj.ContainsKey(key))
                    cap += capAdj[key];
            }
        }
        return cap;
    }
    #endregion
}