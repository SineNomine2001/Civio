using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TopBarUI : UI, IUpdateUI
{
    protected static TopBarUI s_Instance;
    public static TopBarUI Instance
    {
        get
        {
            if (s_Instance != null)
                return s_Instance;
            s_Instance = FindObjectOfType<TopBarUI>();

            return s_Instance;
        }
    }

    public TMP_Text popText;
    public TMP_Text foodText;
    public TMP_Text woodText;
    public TMP_Text stoneText;
    public TMP_Text coinText;

    public void UpdateUI()
    {
        Res resource = GameUI.Instance.player.Resource;
        Res dresource = GameUI.Instance.player.DResource;

        popText.text = dresource.pop >= 0 ? resource.ToString(0) + "<color=green>(" + dresource.pop.ToString("+#;-#;0") + ")</color>" : resource.ToString(0) + "<color=red>(" + dresource.pop.ToString("+#;-#;0") + ")</color>";
        foodText.text = dresource.food >= 0 ? resource.ToString(1) + "<color=green>(" + dresource.food.ToString("+#;-#;0") + ")</color>" : resource.ToString(1) + "<color=red>(" + dresource.food.ToString("+#;-#;0") + ")</color>";
        woodText.text = dresource.wood >= 0 ? resource.ToString(2) + "<color=green>(" + dresource.wood.ToString("+#;-#;0") + ")</color>" : resource.ToString(2) + "<color=red>(" + dresource.wood.ToString("+#;-#;0") + ")</color>";
        stoneText.text = dresource.stone >= 0 ? resource.ToString(3) + "<color=green>(" + dresource.stone.ToString("+#;-#;0") + ")</color>" : resource.ToString(3) + "<color=red>(" + dresource.stone.ToString("+#;-#;0") + ")</color>";
        coinText.text = dresource.coin >= 0 ? resource.ToString(4) + "<color=green>(" + dresource.coin.ToString("+#;-#;0") + ")</color>" : resource.ToString(4) + "<color=red>(" + dresource.coin.ToString("+#;-#;0") + ")</color>";
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
}

