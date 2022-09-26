using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Map generation manager in charge of supplying parameters to the tile automata.
/// ! Not implemented yet !
/// </summary>
public class MapParaUI : CanvasUI<MapParaUI>
{
    public TMP_InputField randSeedInput;
    public Slider riverNumberSlider;
    public Slider riverRndSlider;

    protected override void OnAwake()
    {
        TileAutomata.InitParas();
    }

    public void OnConfirmClick()
    {
        TileAutomata.GenerateMap();
        ToggleVisibility();
    }

    public void OnMapSeedInput()
    {

    }// Delete this and use seeding

    public void OnRiverInfoChange()
    {

    }// Delete this and use seeding
}
