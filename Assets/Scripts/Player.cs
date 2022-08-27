using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Properties
    public Res Resource { get; private set; } = Res.zero;
    public Res DResource { get; private set; } = Res.zero;
    public HashSet<District> Districts { get; private set; } = new HashSet<District>();
    public int TradeCapacity { get; private set; } = 0;
    public int TradeCapacityUsed { get; private set; } = 0;
    public Res TradeResource { get; private set; } = Res.zero;
    public Res TradeCoin { get; private set; } = Res.zero;

    bool isInitialized = false;
    #endregion
    #region Private Functions
    IEnumerator UpdateStatus()
    {
        while (this)
        {
            DResource = TradeResource;
            TradeCapacity = 0;
            foreach (District district in Districts)
            {
                DResource += district.DResource;
                if(district.Type == DistrictType.Town)
                    TradeCapacity += district.Capacity;
            }
            Resource += DResource;
            yield return new WaitForSeconds(1f);
        }
    }

    void CalculateTrade(Res trade)
    {
        TradeCoin = new Res(0, -trade.food, -trade.wood, -trade.stone, -trade.food - trade.wood - trade.stone);
    }
    #endregion
    #region Public Functions
    public Player Init(Vector2Int coord)
    {
        if (!isInitialized)
        {
            Resource = new Res(1000, 1000, 1000, 1000, 1000);
            CreateDistrict("blah", new BuildingData(0, coord));
            StartCoroutine(UpdateStatus());

            isInitialized = true;
        }

        return this;
    }

    public bool ChangeResource(Res change)
    {
        if (Resource >= -change)
        {
            Resource += change;
            return true;
        }
        else
            return false;
    }

    public bool SetTrade(Res change)
    {
        int temp = (int)(Mathf.Abs(change.food) + Mathf.Abs(change.wood) + Mathf.Abs(change.stone));
        if (temp <= TradeCapacity)
        {
            TradeCapacityUsed = temp;
            CalculateTrade(change);
            TradeResource = new Res(0, change.food, change.wood, change.stone, TradeCoin.coin);
            return true;
        }
        else
            return false;
    }

    public PlaceBuildingResult CreateDistrict(string name, BuildingData centerData)
    {
        PlaceBuildingResult result = TileMapManager.Instance.TestBuildingPlacement(centerData);
        if (result == PlaceBuildingResult.Success)
        {
            if (Resource >= StaticData.BDict[centerData.key].cost)
            {
                GameObject go = Instantiate(Resources.Load<GameObject>("Prefabs/District"), transform);
                Districts.Add(go.GetComponent<District>().Init(name, centerData));
            }
            else
                return PlaceBuildingResult.NotAffordable;
        }
        return result;
    }

    public void DeleteDistrict(District district)
    {
        Districts.Remove(district);
        Destroy(district.gameObject);
    }
    #endregion
}
