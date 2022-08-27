using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New Map Rule Tile", menuName = "Tiles/Map Rule Tile")]

public class MapRuleTile : RuleTile
{
    public Res resMultiplier;
}
