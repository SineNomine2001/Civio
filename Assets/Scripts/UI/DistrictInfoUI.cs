using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class DistrictInfoUI : PagedPanelUI, IUpdateUI
{
    protected static DistrictInfoUI s_Instance;
    public static DistrictInfoUI Instance
    {
        get
        {
            if (s_Instance != null)
                return s_Instance;
            s_Instance = FindObjectOfType<DistrictInfoUI>();

            return s_Instance;
        }
    }

    public District ShownDistrict { get; private set; }

    public TMP_Text nameText;
    public TMP_Text outputText;
    public TMP_Text buildingsText;

    public void Show(District district)
    {
        GetComponent<Canvas>().enabled = true;
        ShownDistrict = district;
        allEntryData = ShownDistrict.Buildings.Cast<object>().ToList();
        OnPanelShown();
        UpdateUI();
    }

    public override void Show()
    {
        base.Show();
        allEntryData = ShownDistrict.Buildings.Cast<object>().ToList();
        OnPanelShown();
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (!GetComponent<Canvas>().enabled)
            return;

        nameText.text = "<sprite=" + (int)ShownDistrict.Type + "> " + ShownDistrict.Name;
        outputText.text = "Output: " + ShownDistrict.DResource.ToColouredString();
        buildingsText.text = "Buildings: " + ShownDistrict.Buildings.Count;

        int i = 0;
        while (i < currentEntryData.Count)
        {
            entries[i].Set(currentEntryData[i]);
            i++;
        }
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
}