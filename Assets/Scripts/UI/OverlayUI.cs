using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OverlayUI : UI
{
    protected static OverlayUI s_Instance;
    public static OverlayUI Instance
    {
        get
        {
            if (s_Instance != null)
                return s_Instance;
            s_Instance = FindObjectOfType<OverlayUI>();

            return s_Instance;
        }
    }

    float fadeTime = 1f;

    public TMP_Text WarningText;

    void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        s_Instance = this;
    }

    public IEnumerator ShowText()
    {
        Color temp = WarningText.color;
        temp.a = 0f;
        Color transparent = temp;
        temp.a = 1f;
        Color opaque = temp;

        for (float t = 0.01f; t < fadeTime; t += Time.deltaTime)
        {
            WarningText.color = Color.Lerp(opaque, transparent, t / fadeTime);
            yield return null;
        }
    }

    public void ShowPlacementFailureText(PlaceBuildingResult result)
    {
        Show();
        switch (result)
        {
            case PlaceBuildingResult.ForestMismatch:
                WarningText.text = "Forest Requirement not satisfied!";
                break;
            case PlaceBuildingResult.BlockedByOtherBuildings:
                WarningText.text = "This tile is occupied by another building!";
                break;
            case PlaceBuildingResult.BuildingDistrictTypeMismatch:
                WarningText.text = "This building does not belong to this district!";
                break;
            case PlaceBuildingResult.NotAdjacentToDistrict:
                WarningText.text = "New buildings must be placed adjacent to the district!";
                break;
            case PlaceBuildingResult.NotAffordable:
                WarningText.text = "Not enough resources to construct this building!";
                break;
            default:
                WarningText.text = "Cannot Place Building Here!";
                break;
        }

        StopAllCoroutines();
        StartCoroutine(ShowText());
    }

    public void ShowTradeFailureText()
    {
        Show();
        WarningText.text = "Need more trade capacity!";

        StopAllCoroutines();
        StartCoroutine(ShowText());
    }

    public void ShowFoodBankruptcyText()
    {
        Show();
        WarningText.text = "Not enough food to trade!";

        StopAllCoroutines();
        StartCoroutine(ShowText());
    }

    public void ShowWoodBankruptcyText()
    {
        Show();
        WarningText.text = "Not enough wood to trade!";

        StopAllCoroutines();
        StartCoroutine(ShowText());
    }

    public void ShowStoneBankruptcyText()
    {
        Show();
        WarningText.text = "Not enough stone to trade!";

        StopAllCoroutines();
        StartCoroutine(ShowText());
    }

    public void ShowCoinBankruptcyText()
    {
        Show();
        WarningText.text = "Not enough coin to trade!";

        StopAllCoroutines();
        StartCoroutine(ShowText());
    }
}
