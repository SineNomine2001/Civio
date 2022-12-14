using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BuildingInfoUI : CanvasUI<BuildingInfoUI>, IUpdateUI
{
    public TMP_Text nameText;
    public TMP_Text outputText;
    public TMP_Text hpText;
    public Button destroyBuildingButton;

    Building building;

    public void Show(Building building)
    {
        GetComponent<Canvas>().enabled = true;
        this.building = building;
        destroyBuildingButton.onClick.RemoveAllListeners();
        destroyBuildingButton.onClick.AddListener(() => building.OwnerDistrict.DeleteBuilding(building));
        destroyBuildingButton.onClick.AddListener(() => DistrictInfoUI.Instance.Show());
        destroyBuildingButton.onClick.AddListener(() => Hide());
        if (building.OwnerDistrict.Buildings.Count == 1)
        {
            destroyBuildingButton.onClick.AddListener(() => DistrictInfoUI.Instance.Hide());
            destroyBuildingButton.onClick.AddListener(() => CreateBuilding_UnitUI.Instance.Hide());
        }
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (!GetComponent<Canvas>().enabled)
            return;

        nameText.text = building.Data.stats.name;
        outputText.text = "Output: " + building.Data.stats.dresource.ToColouredString();
        hpText.text = "HP: " + building.HP.CurrentHealth + "/" + building.HP.startingHealth;
    }
}
