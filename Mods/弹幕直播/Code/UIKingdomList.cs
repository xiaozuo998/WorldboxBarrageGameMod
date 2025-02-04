﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIKingdomList : MonoBehaviour
{
    static public UIKingdomList instance;

    public GameObject goMain;
    public GameObject goContent;

    public List<UIKingdom> itemList = new List<UIKingdom>();
    public List<UIKingdom> itemLastList = new List<UIKingdom>();

    private int uiKingdomItemCount = 0;

    private void Awake()
    {
        instance = this;
    }

    // 刷新显示
    public void RefreshDisplay()
    {
        if (goMain == null)
        {
            goMain = new GameObject("KingdomList");
            goMain.transform.SetParent(transform);
            goMain.AddComponent<RectTransform>();
            goMain.AddComponent<Image>().color = Color.clear;
            var rect = goMain.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.one;
            rect.anchorMax = Vector2.one;
            rect.sizeDelta=new Vector2(400, 400);
            rect.anchoredPosition = new Vector2(-5, -20);
            rect.pivot = new Vector2(1f, 1f);
            goMain.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;


            goContent = new GameObject("Content");
            goContent.transform.SetParent(goMain.transform);
            var vert = goContent.AddComponent<VerticalLayoutGroup>();
            vert.childForceExpandHeight = false;
            vert.childForceExpandWidth = true;
            vert.childControlHeight = false;
            rect = goContent.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0, 1);
            rect.anchorMax = new Vector2(0, 1);
            rect.pivot = new Vector2(0f, 1f);
            rect.sizeDelta=new Vector2(400, 0);
            rect.anchoredPosition = new Vector2(0, 0);

        }

    }

    public UIKingdom GetUIKingdom()
    {
        if (itemList.Count <= uiKingdomItemCount)
        {
            var item = new UIKingdom();
            item.RefreshDisplay();
            item.goMain.transform.SetParent(goContent.transform);
            itemList.Add(item);
        }
        UIKingdom ret = itemList[uiKingdomItemCount];
        ret.Clear();
        ret.goMain.transform.SetSiblingIndex(uiKingdomItemCount);
        ret.goMain.SetActive(true);
        ++uiKingdomItemCount;
        UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(goContent.GetComponent<RectTransform>());
        UIKingdomList.instance.ForceRebuildLayout();
        return ret;
    }

    public void RemoveUIKingdom(UIKingdom ui)
    {
        if (ui.goMain.transform.parent != goContent.transform)
        {
            return;
        }
        itemList.Remove(ui);
        ui.goMain.SetActive(false);
        ui.goMain.transform.SetAsLastSibling();
        itemList.Add(ui);
        --uiKingdomItemCount;
        UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(goContent.GetComponent<RectTransform>());
        ForceRebuildLayout();
    }

    public void SetLastUIKingdom(UIKingdom ui)
    {
        if (ui.goMain.transform.parent != goContent.transform)
        {
            return;
        }
        itemList.Remove(ui);
        --uiKingdomItemCount;
        ui.goMain.transform.SetSiblingIndex(uiKingdomItemCount);
        itemLastList.Add(ui);
        UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(goContent.GetComponent<RectTransform>());
        ForceRebuildLayout();
    }


    public void Clear()
    {
        foreach (var item in itemList)
        {
            item.goMain.SetActive(false);
        }
        foreach (var item in itemLastList)
        {
            item.goMain.SetActive(false);
            itemList.Add(item);
        }
        itemLastList.Clear();
        uiKingdomItemCount = 0;
    }

    public void ForceRebuildLayout()
    {
        UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(goMain.GetComponent<RectTransform>());
    }


}
