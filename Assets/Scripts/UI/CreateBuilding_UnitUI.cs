using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CreateBuilding_UnitUI : PagedPanelUI<CreateBuilding_UnitUI>
{
    bool isPanelShown;
    District district;

    public RectTransform rectTransform;
    public Canvas mainCanvas;

    protected override void OnAwake()
    {
        isPanelShown = false;
    }

    public new void ToggleVisibility()
    {
        ToggleVisibility(null, true);
    }

    public void ToggleVisibility(District district, bool isBuilding)
    {
        this.district = district;
        if (isPanelShown)
            Hide();
        else
            Show(isBuilding);
    }

    public void Show(District district, bool isBuilding)
    {
        this.district = district;
        Show(isBuilding);
    }

    public void Show(bool isBuilding)
    {
        if (!isPanelShown)
        {
            transform.position += new Vector3(rectTransform.rect.width * mainCanvas.scaleFactor, 0, 0);
            isPanelShown = true;
        }

        if (district == null)
        {
            if (isBuilding)
                allEntryData = StaticData.BDict.Where(i => i.Key % 100 == 0).Select(j => j.Key).Cast<object>().ToList();
            else
                allEntryData = StaticData.UDict.Where(i => i.Key % 100 == 0).Select(j => j.Key).Cast<object>().ToList();
        }
        else
        {
            if (isBuilding)
            {
                int key = (int)district.Type * 100;
                allEntryData = StaticData.BDict.Where(i => i.Key >= key && i.Key < key + 100).Select(j => j.Key).Cast<object>().ToList();
            }
            else
                allEntryData = new List<object>();
        }

        foreach (PagedEntryUI entry in entries)
        {
            entry.GetComponent<CBUEntryUI>().district = district;
            entry.GetComponent<CBUEntryUI>().isBuilding = isBuilding;
        }

        OnPanelShown();
        GameUI.Instance.RestoreDefault();
    }

    public override void Hide()
    {
        if (isPanelShown)
        {
            transform.position -= new Vector3(rectTransform.rect.width * mainCanvas.scaleFactor, 0, 0);
            isPanelShown = false;
        }
    }
}