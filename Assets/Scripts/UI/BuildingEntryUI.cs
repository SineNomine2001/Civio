using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingEntryUI : PagedEntryUI
{
    public TMP_Text nameText;
    public Button button;

    Building building;

    public override void Set(object para)
    {
        building = (Building)para;
        nameText.text = building.Data.stats.name;
        button.onClick.AddListener(() => CameraMover.Instance.MoveCamera(building.Data.centerPos));
        button.onClick.AddListener(() => BuildingInfoUI.Instance.Show(building));
    }

    public override void Clear()
    {
        building = null;
        nameText.text = "";
        button.onClick.RemoveAllListeners();
    }
}
