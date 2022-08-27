using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : UI
{
    protected static GameUI s_Instance;
    public static GameUI Instance
    {
        get
        {
            if (s_Instance != null)
                return s_Instance;
            s_Instance = FindObjectOfType<GameUI>();

            return s_Instance;
        }
    }

    public Player player;
    Action updateUIs;

    IEnumerator UpdateAllUI()
    {
        while (this)
        {
            updateUIs?.Invoke();
            yield return new WaitForSeconds(1f);
        }
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

    void Start()
    {
        foreach (IUpdateUI updateUI in GetComponentsInChildren<IUpdateUI>())
            updateUIs += updateUI.UpdateUI;
        StartCoroutine("UpdateAllUI");
    }

    public void HideAll()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            child.GetComponent<Canvas>().enabled = false;
        }
    }

    public void HideAllExcept(UI ui)
    {
        HideAll();
        ui.GetComponent<Canvas>().enabled = true;
    }

    public void RestoreDefault()
    {
        HideAll();
        transform.Find("Top Bar UI").GetComponent<Canvas>().enabled = true;
        transform.Find("Create Building_Unit UI").GetComponent<Canvas>().enabled = true;
    }
}
