using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TradeManageUI : UI, IUpdateUI
{
    protected static TradeManageUI s_Instance;
    public static TradeManageUI Instance
    {
        get
        {
            if (s_Instance != null)
                return s_Instance;
            s_Instance = FindObjectOfType<TradeManageUI>();

            return s_Instance;
        }
    }

    public TMP_Text capacityText;
    public TMP_InputField foodTradeText;
    public TMP_Text foodCoinText;
    public TMP_InputField woodTradeText;
    public TMP_Text woodCoinText;
    public TMP_InputField stoneTradeText;
    public TMP_Text stoneCoinText;

    public new void ToggleVisibility()
    {
        base.ToggleVisibility();
        UpdateUI();
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

    void CheckTradeSuccessful(Res tradeResource)
    {
        bool isTradeSuccessful = GameUI.Instance.player.SetTrade(tradeResource);
        if (!isTradeSuccessful)
            OverlayUI.Instance.ShowTradeFailureText();
        UpdateUI();
    }

    public void OnTradeInput()
    {
        try
        {
            CheckTradeSuccessful(new Res(0, int.Parse(foodTradeText.text), int.Parse(woodTradeText.text), int.Parse(stoneTradeText.text), 0));
        }
        catch { }
        UpdateUI();
    }

    void UpdateInputFields()
    {
        Res tradeResource = GameUI.Instance.player.TradeResource;
        foodTradeText.text = tradeResource.food.ToString("+#;-#;0");
        woodTradeText.text = tradeResource.wood.ToString("+#;-#;0");
        stoneTradeText.text = tradeResource.stone.ToString("+#;-#;0");
    }

    public void UpdateUI()
    {
        UpdateInputFields();

        Res tradeCoin = GameUI.Instance.player.TradeCoin;
        capacityText.text = "Capacity: " + GameUI.Instance.player.TradeCapacityUsed + "/" + GameUI.Instance.player.TradeCapacity;
        foodCoinText.text = tradeCoin.food.ToString("+#;-#;0");
        woodCoinText.text = tradeCoin.wood.ToString("+#;-#;0");
        stoneCoinText.text = tradeCoin.stone.ToString("+#;-#;0");
    }

    #region buttons
    public void IncrementFood()
    {
        Res tradeResource = GameUI.Instance.player.TradeResource;
        if (Input.GetKey(StaticData.x10) && Input.GetKey(StaticData.x100))
            tradeResource += new Res(0, 1000, 0, 0, 0);
        else if (Input.GetKey(StaticData.x100))
            tradeResource += new Res(0, 100, 0, 0, 0);
        else if (Input.GetKey(StaticData.x10))
            tradeResource += new Res(0, 10, 0, 0, 0);
        else
            tradeResource += new Res(0, 1, 0, 0, 0);

        CheckTradeSuccessful(tradeResource);
    }

    public void DecrementFood()
    {
        Res tradeResource = GameUI.Instance.player.TradeResource;
        if (Input.GetKey(StaticData.x10) && Input.GetKey(StaticData.x100))
            tradeResource += new Res(0, -1000, 0, 0, 0);
        else if (Input.GetKey(StaticData.x100))
            tradeResource += new Res(0, -100, 0, 0, 0);
        else if (Input.GetKey(StaticData.x10))
            tradeResource += new Res(0, -10, 0, 0, 0);
        else
            tradeResource += new Res(0, -1, 0, 0, 0);

        CheckTradeSuccessful(tradeResource);
    }

    public void ClearFood()
    {
        Res tradeResource = GameUI.Instance.player.TradeResource;
        tradeResource = new Res(tradeResource.pop, 0, tradeResource.wood, tradeResource.stone, tradeResource.coin);

        CheckTradeSuccessful(tradeResource);
    }

    public void IncrementWood()
    {
        Res tradeResource = GameUI.Instance.player.TradeResource;
        if (Input.GetKey(StaticData.x10) && Input.GetKey(StaticData.x100))
            tradeResource += new Res(0, 0, 1000, 0, 0);
        else if (Input.GetKey(StaticData.x100))
            tradeResource += new Res(0, 0, 100, 0, 0);
        else if (Input.GetKey(StaticData.x10))
            tradeResource += new Res(0, 0, 10, 0, 0);
        else
            tradeResource += new Res(0, 0, 1, 0, 0);

        CheckTradeSuccessful(tradeResource);
    }

    public void DecrementWood()
    {
        Res tradeResource = GameUI.Instance.player.TradeResource;
        if (Input.GetKey(StaticData.x10) && Input.GetKey(StaticData.x100))
            tradeResource += new Res(0, 0, -1000, 0, 0);
        else if (Input.GetKey(StaticData.x100))
            tradeResource += new Res(0, 0, -100, 0, 0);
        else if (Input.GetKey(StaticData.x10))
            tradeResource += new Res(0, 0, -10, 0, 0);
        else
            tradeResource += new Res(0, 0, -1, 0, 0);

        CheckTradeSuccessful(tradeResource);
    }

    public void ClearWood()
    {
        Res tradeResource = GameUI.Instance.player.TradeResource;
        tradeResource = new Res(tradeResource.pop, tradeResource.food, 0, tradeResource.stone, tradeResource.coin);

        CheckTradeSuccessful(tradeResource);
    }

    public void IncrementStone()
    {
        Res tradeResource = GameUI.Instance.player.TradeResource;
        if (Input.GetKey(StaticData.x10) && Input.GetKey(StaticData.x100))
            tradeResource += new Res(0, 0, 0, 1000, 0);
        else if (Input.GetKey(StaticData.x100))
            tradeResource += new Res(0, 0, 0, 100, 0);
        else if (Input.GetKey(StaticData.x10))
            tradeResource += new Res(0, 0, 0, 10, 0);
        else
            tradeResource += new Res(0, 0, 0, 1, 0);

        CheckTradeSuccessful(tradeResource);
    }

    public void DecrementStone()
    {
        Res tradeResource = GameUI.Instance.player.TradeResource;
        if (Input.GetKey(StaticData.x10) && Input.GetKey(StaticData.x100))
            tradeResource += new Res(0, 0, 0, -1000, 0);
        else if (Input.GetKey(StaticData.x100))
            tradeResource += new Res(0, 0, 0, -100, 0);
        else if (Input.GetKey(StaticData.x10))
            tradeResource += new Res(0, 0, 0, -10, 0);
        else
            tradeResource += new Res(0, 0, 0, -1, 0);

        CheckTradeSuccessful(tradeResource);
    }

    public void ClearStone()
    {
        Res tradeResource = GameUI.Instance.player.TradeResource;
        tradeResource = new Res(tradeResource.pop, tradeResource.food, tradeResource.wood, 0, tradeResource.coin);

        CheckTradeSuccessful(tradeResource);
    }
    #endregion
}
