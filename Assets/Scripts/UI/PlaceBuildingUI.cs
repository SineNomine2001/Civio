using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlaceBuildingUI : UI
{
    protected static PlaceBuildingUI s_Instance;
    public static PlaceBuildingUI Instance
    {
        get
        {
            if (s_Instance != null)
                return s_Instance;
            s_Instance = FindObjectOfType<PlaceBuildingUI>();

            return s_Instance;
        }
    }

    public PBPreview preview;
    public TMP_Text cancelText;

    int key;
    District district;
    Vector3 mousePositionWorld;

    public void Show(int key, District district)
    {
        this.key = key;
        this.district = district;
        preview.Set(key);
        preview.Show();
        cancelText.text = "Press " + StaticData.cancel + " or " + StaticData.exit + " to cancel.";
        GameUI.Instance.HideAllExcept(this);
    }

    public override void Hide()
    {
        GameUI.Instance.RestoreDefault();
        preview.Hide();
    }

    void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        s_Instance = this;
    }

    void Update()
    {
        if (GetComponent<Canvas>().enabled)
        {
            mousePositionWorld = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
            preview.SetLocalPosition(mousePositionWorld);

            if (Input.GetKey(StaticData.cancel) || Input.GetKey(StaticData.exit))
            {
                Hide();
            }

            if (Input.GetKey(StaticData.confirm))
            {
                PlaceBuilding();
            }
        }
    }

    void PlaceBuilding()
    {
        Vector3Int pos = TileMapManager.Instance.grid.WorldToCell(preview.transform.position);
        Vector2Int coord = new Vector2Int(pos.x, pos.y);
        BuildingData data = new BuildingData(key, coord);
        PlaceBuildingResult result;
        Building building = null;

        if (district == null)
            result = GameUI.Instance.player.CreateDistrict("blah", data);
        else
            result = district.PlaceBuilding(data, out building);

        if (result == PlaceBuildingResult.Success)
        {
            Hide();
            if (building != null)
                building.OnMouseDown();
        }
        else
        {
            OverlayUI.Instance.ShowPlacementFailureText(result);
        }
    }
}
