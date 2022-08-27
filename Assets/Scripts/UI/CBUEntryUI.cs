using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CBUEntryUI : PagedEntryUI
{
    public Image icon;
    public TMP_Text nameText;
    public TMP_Text costText;
    public Button button;
    public District district;
    public bool isBuilding;

    public override void Set(object para)
    {
        int key = (int)para;
        BuildingStats building = StaticData.BDict[key];
        icon.sprite = StaticData.GetBuildingSprite(key);
        Color temp = icon.color;
        temp.a = 1f;
        icon.color = temp;
        nameText.text = building.name;
        costText.text = building.cost.ToString();
        button.onClick.AddListener(() => PlaceBuildingUI.Instance.Show(key, district));
    }

    public override void Clear()
    {
        icon.sprite = null;
        Color temp = icon.color;
        temp.a = 0f;
        icon.color = temp;
        nameText.text = "";
        costText.text = "";
        button.onClick.RemoveAllListeners();
    }
}
