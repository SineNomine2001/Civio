using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PagedPanelUI : UI
{
    public RectTransform panel;
    public TMP_Text totalPageText;
    public TMP_InputField currentPageInput;
    protected PagedEntryUI[] entries;
    protected List<object> currentEntryData = new List<object>();
    protected List<object> allEntryData = new List<object>();
    protected List<List<object>> pagedEntryData = new List<List<object>>();
    protected int totalPage;
    protected int currentPage;

    public void ShowEntries()
    {
        currentEntryData = pagedEntryData[currentPage - 1];
        currentPageInput.text = currentPage.ToString();
        int i = 0;
        while (i < currentEntryData.Count)
        {
            entries[i].Set(currentEntryData[i]);
            i++;
        }
        while (i < entries.Length)
        {
            entries[i].Clear();
            i++;
        }
    }

    public void OnPanelShown()
    {
        if (GetComponent<Canvas>().enabled && allEntryData.Count != 0)
        {
            totalPage = 0;
            pagedEntryData = new List<List<object>>();
            int index = 0;
            int count = Mathf.Min(entries.Length, allEntryData.Count);
            while (index < allEntryData.Count)
            {
                totalPage++;
                pagedEntryData.Add(allEntryData.GetRange(index, count));
                index += count;
                count = Mathf.Min(entries.Length, allEntryData.Count - totalPage * entries.Length);
            }
            currentPage = 1;
            totalPageText.text = "/" + totalPage;
            ShowEntries();
        }
    }

    public void PageUp()
    {
        currentPage = Mathf.Max(currentPage - 1, 1);
        ShowEntries();
    }

    public void PageDown()
    {
        currentPage = Mathf.Min(currentPage + 1, totalPage);
        ShowEntries();
    }

    public void OnPageInput()
    {
        try
        {
            currentPage = Mathf.Clamp(int.Parse(currentPageInput.text), 1, totalPage);
            ShowEntries();
        }
        catch
        {
            currentPageInput.text = currentPage.ToString();
        }
    }

    protected virtual void Awake()
    {
        entries = panel.GetComponentsInChildren<PagedEntryUI>();
    }
}