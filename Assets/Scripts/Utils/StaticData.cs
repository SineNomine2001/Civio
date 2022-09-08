using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class StaticData
{
    #region Tiles
    public static readonly MapRuleTile GrassTile = Resources.Load<MapRuleTile>("Tiles/Grass");
    public static readonly MapRuleTile WaterTile = Resources.Load<MapRuleTile>("Tiles/Water");
    public static readonly MapRuleTile RockyTile = Resources.Load<MapRuleTile>("Tiles/Rocky");
    public static readonly MapRuleTile GlacierTile = Resources.Load<MapRuleTile>("Tiles/Glacier");

    public static readonly MapRuleTile ForestTile = Resources.Load<MapRuleTile>("Tiles/Forest");
    public static readonly MapRuleTile MangroveTile = Resources.Load<MapRuleTile>("Tiles/Mangrove");
    public static readonly MapRuleTile AlpineTile = Resources.Load<MapRuleTile>("Tiles/Alpine");

    public static readonly RuleTile FogOfWarTile = Resources.Load<RuleTile>("Tiles/FogOfWar");
    #endregion
    #region KeyBindings
    public static KeyCode confirm = KeyCode.Mouse0;
    public static KeyCode cancel = KeyCode.Mouse1;
    public static KeyCode exit = KeyCode.Escape;
    public static KeyCode previous = KeyCode.Q;
    public static KeyCode next = KeyCode.E;
    public static KeyCode x10 = KeyCode.LeftShift;
    public static KeyCode x100 = KeyCode.LeftControl;
    public static KeyCode Entry1 = KeyCode.Alpha1;
    public static KeyCode Entry2 = KeyCode.Alpha2;
    public static KeyCode Entry3 = KeyCode.Alpha3;
    public static KeyCode Entry4 = KeyCode.Alpha4;
    public static KeyCode Entry5 = KeyCode.Alpha5;
    public static KeyCode Page1 = KeyCode.F1;
    public static KeyCode Page2 = KeyCode.F2;
    public static KeyCode Page3 = KeyCode.F3;
    public static KeyCode Page4 = KeyCode.F4;
    public static KeyCode Page5 = KeyCode.F5;
    #endregion

    // ID: 3 digits, Type-Tier-#
    // Infantries require [Tier-1] Blacksmith
    // Archers require [Tier] Blacksmith
    // Cavalries require [Tier] Blacksmith & [Tier+1] Military Center
    public static readonly Dictionary<int, UnitStats> UDict = new Dictionary<int, UnitStats>()
    {
        {000, new UnitStats("Militia", 10, 2, 1.0f, new Res(1,5,5,0,0))},
        {010, new UnitStats("Spearman", 20, 3, 1.0f, new Res(1,15,15,5,0))},
        {020, new UnitStats("Pavisier", 25, 3, 1.0f, new Res(1,15,20,15,0))},
        {030, new UnitStats("Pikeman", 34, 6, 1.0f, new Res(1,30,30,25,0))},
        {040, new UnitStats("Greatswordsman", 50, 15, 1.0f, new Res(1,60,60,60,0))},

        {100, new UnitStats("Archer", 5, 2, 1.0f, new Res(1,7,20,10,0))},
        {110, new UnitStats("Crossbowman", 9, 4, 1.0f, new Res(1,12,35,15,0))},
        {120, new UnitStats("Repeater", 15, 4, 1.0f, new Res(1,20,55,20,0))},
        {130, new UnitStats("Muskteer", 22, 30, 1.0f, new Res(1,35,80,30,0))},

        {200, new UnitStats("Rider", 45, 7, 2.0f, new Res(1,65,15,15,0))},
        {210, new UnitStats("Cataphract", 70, 10, 2.0f, new Res(1,120,30,60,0))},
        {220, new UnitStats("Knight", 120, 17, 2.0f, new Res(1,260,50,170,0))},
    };

    #region Building output by adjacent buildings
    // Town
    static readonly Dictionary<int, int> Market_capAdjDict = new Dictionary<int, int>
    {
        {001,5},
        {022,20},
    };
    static readonly Dictionary<int, Res> Tavern_drAdjDict = new Dictionary<int, Res>
    {
        {001,new Res(1,-2,0,0,0)},
        {022,new Res(3,-5,0,0,0)},
    };
    static readonly Dictionary<int, Res> Bank_drAdjDict = new Dictionary<int, Res>
    {
        {001,new Res(0,0,0,0,10)},
        {022,new Res(0,0,0,0,20)},
    };
    static readonly Dictionary<int, int> Bank_capAdjDict = new Dictionary<int, int>
    {
        {001,5},
        {022,15},
    };
    static readonly Dictionary<int, Res> Garden_drAdjDict = new Dictionary<int, Res>
    {
        {001,new Res(4,-6,0,0,0)},
        {022,new Res(13,-23,0,0,0)},
    };
    static readonly Dictionary<int, Res> GrandBazaar_drAdjDict = new Dictionary<int, Res>
    {
        {001,new Res(0,0,0,0,12)},
        {011,new Res(0,50,50,50,-20)},
        {022,new Res(0,0,0,0,30)},
    };
    static readonly Dictionary<int, int> GrandBazaar_capAdjDict = new Dictionary<int, int>
    {
        {001,20},
        {022,50},
    };
    static readonly Dictionary<int, Res> Fountain_drAdjDict = new Dictionary<int, Res>
    {
        {001,new Res(10,-16,0,0,0)},
        {022,new Res(20,-40,0,0,0)},
    };

    // Production
    static readonly Dictionary<int, Res> Mill_drAdjDict = new Dictionary<int, Res>
    {
        {101,new Res(0,15,0,0,0)},
    };
    static readonly Dictionary<int, Res> DoubleSaw_drAdjDict = new Dictionary<int, Res>
    {
        {102,new Res(0,0,5,0,0)},
    };
    static readonly Dictionary<int, Res> MineCart_drAdjDict = new Dictionary<int, Res>
    {
        {103,new Res(0,0,0,5,0)},
    };
    static readonly Dictionary<int, Res> LumberMill_drAdjDict = new Dictionary<int, Res>
    {
        {112,new Res(0,0,30,0,0)},
    };
    static readonly Dictionary<int, Res> Forge_drAdjDict = new Dictionary<int, Res>
    {
        {103,new Res(0,0,0,15,0)},
        {113,new Res(0,0,0,25,0)},
    };
    static readonly Dictionary<int, Res> DiaryShop_drAdjDict = new Dictionary<int, Res>
    {
        {121,new Res(0,100,0,0,0)},
    };
    static readonly Dictionary<int, Res> AssemblyPlant_drAdjDict = new Dictionary<int, Res>
    {
        {122,new Res(0,0,90,90,0)},
        {123,new Res(0,0,90,90,0)},
    };

    // Military
    static readonly Dictionary<int, int> Armoury_capAdjDict = new Dictionary<int, int>
    {
        {201,8},
        {211,5},
        {212,10},
        {221,15},
        {231,20},
    };
    #endregion

    // ID: 3 digits, District-Tier-#, IDs ending in 0 are district centers
    // Small buildings for early-mid game and filling in holes
    // Medium weirdly-shaped buildings that produce depending on surroundings
    // Large buildings for mid-late game

    // Pop: Houses and manors surrounding commerce and entertainment buildings
    // Food: Farms surrounding windmills, pastures surrounding diary shops
    // Wood: Chopping yard goes to double saw, which goes to lumber mill, which goes to assembly plant
    // Stone: Mining camp goes to mine cart, both go to forge, which goes to assembly plant
    // Military: Armouries provide soldier capacity for adjacent blacksmiths and barracks
    public static readonly Dictionary<int, BuildingStats> BDict = new Dictionary<int, BuildingStats>()
    {
        {000, new BuildingStats("Town Center lv0", new Vector2Int(3,3), false, 75, new Res(10,100,100,150,0), Res.zero)},
        {001, new BuildingStats("House", new Vector2Int(1,1), false, 30, new Res(5,15,25,25,0), new Res(1,-1,0,0,0), 0)},
        {010, new BuildingStats("Town Center lv1", new Vector2Int(3,3), false, 150, new Res(20,250,250,400,0), Res.zero)},
        {011, new BuildingStats("Market", new Vector2Int(1,1), false, 30, new Res(5,25,60,40,0), Res.zero, 20, null, Market_capAdjDict)},
        {012, new BuildingStats("Tavern", new Vector2Int(2,1), false, 75, new Res(8,70,130,80,0), Res.zero, 0, Tavern_drAdjDict)},
        {020, new BuildingStats("Town Center lv2", new Vector2Int(3,3), false, 600, new Res(50,500,500,900,0), Res.zero)},
        {021, new BuildingStats("Bank", new Vector2Int(2,2), false, 400, new Res(16,0,250,500,500), new Res(0,0,0,0,50), 50, Bank_drAdjDict, Bank_capAdjDict)},
        {022, new BuildingStats("Manor", new Vector2Int(2,2), false, 250, new Res(12,250,350,350,0), new Res(7,-10,0,0,0), 0)},
        {023, new BuildingStats("Garden", new Vector2Int(3,2), false, 320, new Res(6,0,650,500,0), new Res(0,0,-12,-10,0), 0, Garden_drAdjDict)},
        {030, new BuildingStats("Town Center lv3", new Vector2Int(3,3), false, 1200, new Res(100,2000,2000,3500,0), Res.zero)},
        {031, new BuildingStats("Grand Bazaar", new Vector2Int(4,3), false, 900, new Res(70,0,800,600,800), new Res(0,0,0,0,100), 500, GrandBazaar_drAdjDict, GrandBazaar_capAdjDict)},
        {032, new BuildingStats("Fountain", new Vector2Int(3,3), false, 700, new Res(25,0,550,900,0), new Res(0,0,-15,-20,0), 0, Fountain_drAdjDict)},

        {100, new BuildingStats("Production Center lv0", new Vector2Int(3,3), false, 60, new Res(8,60,60,60,0), Res.zero)},
        {101, new BuildingStats("Farm", new Vector2Int(2,2), false, 50, new Res(2,0,20,0,0), new Res(0,8,0,0,0))},
        {102, new BuildingStats("Chopping Yard", new Vector2Int(1,1), true, 20, new Res(2,0,10,10,0), new Res(0,0,5,0,0))},
        {103, new BuildingStats("Mining Camp", new Vector2Int(1,1), false, 20, new Res(2,0,5,15,0), new Res(0,0,0,5,0))},
        {110, new BuildingStats("Production Center lv1", new Vector2Int(3,3), false, 100, new Res(12,100,100,100,0), Res.zero)},
        {111, new BuildingStats("Windmill", new Vector2Int(1,1), false, 30, new Res(4,0,10,50,0), Res.zero, 50, Mill_drAdjDict)},
        {112, new BuildingStats("Double Saw", new Vector2Int(1,2), true, 60, new Res(6,0,45,45,0), new Res(0,0,15,0,0), 0, DoubleSaw_drAdjDict)},
        {113, new BuildingStats("Mine cart", new Vector2Int(2,1), false, 60, new Res(6,0,15,75,0), new Res(0,0,0,15,0), 0, MineCart_drAdjDict)},
        {120, new BuildingStats("Production Center lv2", new Vector2Int(3,3), false, 300, new Res(20,300,300,300,0), Res.zero)},
        {121, new BuildingStats("Pasture", new Vector2Int(4,4), false, 250, new Res(12,0,250,0,0), new Res(0,60,0,0,0))},
        {122, new BuildingStats("Lumber Mill", new Vector2Int(3,2), true, 120, new Res(12,0,150,150,0), new Res(0,0,40,0,0), 0, LumberMill_drAdjDict)},
        {123, new BuildingStats("Forge", new Vector2Int(2,3), false, 170, new Res(12,0,100,200,0), new Res(0,0,0,40,0), 0, Forge_drAdjDict)},
        {130, new BuildingStats("Production Center lv3", new Vector2Int(3,3), false, 700, new Res(40,800,800,800,0), Res.zero)},
        {131, new BuildingStats("Diary Shop", new Vector2Int(2,1), false, 350, new Res(25,0,150,400,0), Res.zero, 0, DiaryShop_drAdjDict)},
        {132, new BuildingStats("Assembly Plant", new Vector2Int(3,4), false, 500, new Res(40,0,1200,1000,0), new Res(0,0,150,150,0), 0, AssemblyPlant_drAdjDict)},

        {200, new BuildingStats("Military Center lv0", new Vector2Int(3,3), false, 120, new Res(8,40,100,100,0), Res.zero)},
        {201, new BuildingStats("Barracks", new Vector2Int(1,2), false, 40, new Res(2,0,40,10,0), Res.zero, 5)},
        {210, new BuildingStats("Military Center lv1", new Vector2Int(3,3), false, 350, new Res(12,80,150,150,0), Res.zero)},
        {211, new BuildingStats("Blacksmith lv0", new Vector2Int(2,1), false, 100, new Res(3,0,200,100,0), Res.zero, 8)},
        {212, new BuildingStats("Blacksmith lv1", new Vector2Int(2,1), false, 175, new Res(10,0,200,300,0), Res.zero, 16)},
        {213, new BuildingStats("Stable", new Vector2Int(2,2), false, 120, new Res(5,50,100,15,0), Res.zero, 6, null)},
        {220, new BuildingStats("Military Center lv2", new Vector2Int(3,3), false, 800, new Res(20,200,400,400,0), Res.zero)},
        {221, new BuildingStats("Blacksmith lv2", new Vector2Int(2,1), false, 300, new Res(20,0,800,500,0), Res.zero, 32)},
        {222, new BuildingStats("Armoury", new Vector2Int(2,2), false, 600, new Res(10,0,250,350,0), Res.zero, 15, null, Armoury_capAdjDict)},
        {230, new BuildingStats("Military Center lv3", new Vector2Int(3,3), false, 1500, new Res(40,500,1200,1200,0), Res.zero)},
        {231, new BuildingStats("Blacksmith lv3", new Vector2Int(2,1), false, 700, new Res(30,0,1000,1400,0), Res.zero, 64)},
        {232, new BuildingStats("Jousting ground", new Vector2Int(2,1), false, 450, new Res(20,200,650,300,0), Res.zero, 35)},
    };

    public static string IndexToString(int key)
    {
        if (key < 10)
            return "00" + key.ToString();
        else if (key < 100)
            return "0" + key.ToString();
        else
            return key.ToString();
    }

    public static Sprite GetBuildingSprite(int key)
    {
        string keystr = IndexToString(key);
        return Resources.Load<Sprite>("Sprites/Buildings/" + keystr);
    }

}

#region Enums
public enum DistrictType
{
    Town,
    Production,
    Military
}

public enum PlaceBuildingResult
{
    Success,
    ForestMismatch,
    BlockedByOtherBuildings,
    BuildingDistrictTypeMismatch,
    NotAdjacentToDistrict,
    NotAffordable
}
#endregion
#region Structs
public struct UnitStats
{
    public readonly string name;
    public readonly int hp;
    public readonly int damage;
    public readonly float speed;
    public readonly Res cost;

    internal UnitStats(string name, int hp, int damage, float speed, Res cost)
    {
        this.name = name;
        this.damage = damage;
        this.hp = hp;
        this.speed = speed;
        this.cost = cost;
    }
}

public struct BuildingStats
{
    public readonly string name;
    public readonly Vector2Int size;
    public readonly bool isForest;

    public readonly int hp;
    public readonly Res cost;
    public readonly Res dresource;

    public readonly int capacity;

    public readonly Dictionary<int,Res> drAdj;
    public readonly Dictionary<int,int> capAdj;

    internal BuildingStats(string name, Vector2Int size, bool isForest, int hp, Res cost, Res dresource, int capacity = 0, Dictionary<int, Res> drAdj = null, Dictionary<int, int> capAdj = null)
    {
        this.name = name;
        this.size = size;
        this.isForest = isForest;

        this.hp = hp;
        this.cost = cost;
        this.dresource = dresource;
        this.capacity = capacity;

        this.drAdj = drAdj;
        this.capAdj = capAdj;
    }
}

public struct BuildingData
{
    public readonly int key;
    public readonly BuildingStats stats;
    public readonly Vector2Int bottomLeftCorner;
    public readonly Vector2 centerPos;
    public readonly HashSet<Vector2Int> occupiedTiles;
    public readonly HashSet<Vector2Int> adjacentTiles;

    public BuildingData(int key, Vector2Int bottomLeftCorner)
    {
        this.key = key;
        this.bottomLeftCorner = bottomLeftCorner;

        stats = StaticData.BDict[key];
        centerPos = bottomLeftCorner + (Vector2)stats.size / 2;

        Vector2Int topRightCorner = bottomLeftCorner + stats.size;
        occupiedTiles = new HashSet<Vector2Int>();
        adjacentTiles = new HashSet<Vector2Int>();
        for (int x = bottomLeftCorner.x - 1; x < topRightCorner.x + 1; x++)
        {
            for (int y = bottomLeftCorner.y - 1; y < topRightCorner.y + 1; y++)
            {
                Vector2Int tile = new Vector2Int(x, y);
                if (x >= bottomLeftCorner.x && x < topRightCorner.x && y >= bottomLeftCorner.y && y < topRightCorner.y)
                    occupiedTiles.Add(tile);
                else
                    adjacentTiles.Add(tile);
            }
        }
    }
}
#endregion