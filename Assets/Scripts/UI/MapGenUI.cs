using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Map generation manager in charge of supplying parameters to the tile automata
/// </summary>
public class MapGenUI : UI
{
    protected static MapGenUI s_Instance;
    public static MapGenUI Instance
    {
        get
        {
            if (s_Instance != null)
                return s_Instance;
            s_Instance = FindObjectOfType<MapGenUI>();

            return s_Instance;
        }
    }

    public TMP_InputField randSeedInput;
    public Slider riverNumberSlider;
    public Slider riverRndSlider;

    void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        s_Instance = this;
        
        TileAutomata.InitParas();
    }

    void Update()
    {
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
