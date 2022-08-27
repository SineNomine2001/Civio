using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DistrictEntryUI : PagedEntryUI
{
    public TMP_Text nameText;
    public TMP_Text outputText;
    public TMP_Text buildingsText;
    public Button button;

    District district;

    public override void Set(object para)
    {
        district = (District)para;
        nameText.text = "<sprite=" + (int)district.Type + "> " + district.Name;
        outputText.text = district.DResource.ToColouredString();
        buildingsText.text = district.Buildings.Count.ToString();
        button.onClick.AddListener(() => DistrictInfoUI.Instance.Show(district));
    }

    public override void Clear()
    {
        district = null;
        nameText.text = "";
        outputText.text = "";
        buildingsText.text = "";
        button.onClick.RemoveAllListeners();
    }
}
