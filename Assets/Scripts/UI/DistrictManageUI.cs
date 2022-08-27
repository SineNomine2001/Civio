using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DistrictManageUI : PagedPanelUI, IUpdateUI
{
    protected static DistrictManageUI s_Instance;
    public static DistrictManageUI Instance
    {
        get
        {
            if (s_Instance != null)
                return s_Instance;
            s_Instance = FindObjectOfType<DistrictManageUI>();

            return s_Instance;
        }
    }

    public TMP_Text countText;

    public new void ToggleVisibility()
    {
        base.ToggleVisibility();

        allEntryData = GameUI.Instance.player.Districts.Cast<object>().ToList();
        countText.text = "Count: " + allEntryData.Count;
        OnPanelShown();
    }

    protected override void Awake()
    {
        base.Awake();
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        s_Instance = this;
    }

    public void UpdateUI()
    {
        int i = 0;
        while (i < currentEntryData.Count)
        {
            entries[i].Set(currentEntryData[i]);
            i++;
        }
    }
}