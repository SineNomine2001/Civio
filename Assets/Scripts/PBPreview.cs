using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PBPreview : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    BuildingStats building;

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Set(int key)
    {
        spriteRenderer.sprite = StaticData.GetBuildingSprite(key);
        building = StaticData.BDict[key];
    }

    public void SetLocalPosition(Vector2 position)
    {
        transform.localPosition = Vector3Int.RoundToInt(position - (Vector2)building.size / 2);
    }
}
