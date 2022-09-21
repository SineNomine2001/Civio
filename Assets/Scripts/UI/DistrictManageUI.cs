using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DistrictManageUI : PagedPanelUI<DistrictManageUI>, IUpdateUI
{
    public TMP_Text countText;

    public new void ToggleVisibility()
    {
        base.ToggleVisibility();

        allEntryData = GameUI.Instance.player.Districts.Cast<object>().ToList();
        countText.text = "Count: " + allEntryData.Count;
        OnPanelShown();
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