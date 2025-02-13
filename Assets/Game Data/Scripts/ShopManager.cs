using System;
using System.Collections;
using System.Collections.Generic;
using Ommy.Audio;
using Ommy.SaveData;
using Ommy.Singleton;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopManager : Singleton<ShopManager>
{
    public CurrencyCollectionAnimation currencyCollectionAnimation;
    public GameObject notEnoughCurrencyLog;
    public List<ShopItem> shopItems;
    [Header("UI Objects")]
    public Image previewImg;
    public ShopItemUI selectedItemUI;
    public List<ShopItemUI> shopItemUIs;
    [Header("Buttons")]
    public GameObject equip;
    public GameObject equipped, getIt, getItAd, get50Fish;
    public  UnityEvent onUnlockItem, onEquipItem;
    public void OnEnable()
    {
        OnEnableDelay();
        //Invoke(nameof(OnEnableDelay),1);
    }
    private void OnEnableDelay()
    {
        FetchData();
        SetupItems();
        Init();
    }
    public void Init()
    {
        selectedItemUI = shopItemUIs.Find(f=>f.shopItem.shopItemData.hasSelected==true);
        UpdatePanel(selectedItemUI);
    }
    public void FetchData()
    {
        SaveSystem.LoadProgress();
        for (int i = 0; i < shopItems.Count; i++)
        {
            var data = SaveData.Instance.shopItemDatas.Find(
                f => f.shopItemType == shopItems[i].shopItemData.shopItemType
            );
            if (data != null) shopItems[i].shopItemData = data;
            if (data == null) SaveData.Instance.shopItemDatas.Add(shopItems[i].shopItemData);
        }
    }
    public void SaveShopData()
    {
        foreach (var item in shopItems)
        {
            var data = SaveData.Instance.shopItemDatas.Find
            (
                f => f.shopItemType == item.shopItemData.shopItemType
            );
            if (data != null) data = item.shopItemData;
            if (data == null) SaveData.Instance.shopItemDatas.Add(item.shopItemData);
        }
        SaveSystem.SaveProgress();
    }
    public void SetupItems()
    {
        for (int i = 0; i < shopItemUIs.Count; i++)
        {
            shopItemUIs[i].Setup(shopItems[i]);
        }
    }
    public void OnShopItemClick(ShopItemUI shopItemUI)
    {
        AudioManager.Instance?.PlaySFX(SFX.Click);
        UpdatePanel(shopItemUI);
    }
    public void UpdatePanel(ShopItemUI _shopItemUI)
    {
        selectedItemUI = _shopItemUI;
        shopItemUIs.ForEach(f => f.Hightlight(false));
        selectedItemUI.Hightlight(true);
        previewImg.sprite = selectedItemUI.shopItem.icon;
        if (selectedItemUI.shopItem.shopItemData.hasSelected)
        {
            SetButtonActive(equipped);
        }
        else if (selectedItemUI.shopItem.shopItemData.hasUnlocked)
        {
            SetButtonActive(equip);
        }
        else if (selectedItemUI.shopItem.currencyType == CurrencyType.ad)
        {
            SetButtonActive(getItAd);
        }
        else
        {
            SetButtonActive(getIt);
        }
    }
    public void SetButtonActive(GameObject buttonObj)
    {
        getIt.SetActive(false);
        getItAd.SetActive(false);
        equip.SetActive(false);
        equipped.SetActive(false);

        buttonObj.SetActive(true);
    }
    public void Equip()
    {
        AudioManager.Instance?.PlaySFX(SFX.Click);
        SetButtonActive(equipped);
        SelectShopItem(selectedItemUI.shopItem);
        onEquipItem.Invoke();
    }
    public void GetAdFish(int fish)
    {
        AudioManager.Instance?.PlaySFX(SFX.Click);
        //AdsManager.ShowRewardedAd(()=>OnAdFishSucess(), "Fish");
    }
    public UnityEvent onAddFishes;
    public void OnAdFishSucess()
    {
        int fishs = GameManager.Instance.GetFishes();
        fishs += 50;
        GameManager.Instance.SetFishes(fishs);
        currencyCollectionAnimation.CollectCash();
        onAddFishes.Invoke();
    }
    public void GetIt()
    {
        AudioManager.Instance?.PlaySFX(SFX.Click);

        CurrencyType currencyType = selectedItemUI.shopItem.currencyType;
        if (currencyType == CurrencyType.fish)
        {
            if (GameManager.Instance.GetFishes() >= selectedItemUI.shopItem.currencyValue)
            {
                int fishs = GameManager.Instance.GetFishes();
                fishs -= selectedItemUI.shopItem.currencyValue;
                GameManager.Instance.SetFishes(fishs);
                UnlockShopItem(selectedItemUI);
                SetButtonActive(equip);
            }
            else
            {
                NotEnoughCurrency(currencyType);
            }
        }
        else if (currencyType == CurrencyType.key)
        {
            if (GameManager.Instance.GetKeys() >= selectedItemUI.shopItem.currencyValue)
            {
                int keys = GameManager.Instance.GetKeys();
                keys = keys-selectedItemUI.shopItem.currencyValue;
                GameManager.Instance.SetKeys(keys);
                SetButtonActive(equip);
                UnlockShopItem(selectedItemUI);
            }
            else
            {
                NotEnoughCurrency(currencyType);
            }
        }
    }
    public void SelectShopItem(ShopItem shopItem)
    {
        shopItems.ForEach(f => f.shopItemData.hasSelected = false);
        shopItem.shopItemData.hasSelected = true;
        SaveShopData();
    }
    public void UnlockShopItem(ShopItemUI shopItemUI)
    {
        shopItemUI.shopItem.shopItemData.hasUnlocked = true;
        shopItemUI.OnUnlocked();
        SaveShopData();
        SetAnalyticsEvent(shopItemUI.shopItem.shopItemData.shopItemType);
        onUnlockItem.Invoke();
    }
    public void SetAnalyticsEvent(ShopItemType shopItemType)
    {
        switch (shopItemType)
        {
            case ShopItemType.bad2:
                //AdsManager.SendFirebaseEevents("Bed_2_Unlock");
                break;
            case ShopItemType.bad3:
                //AdsManager.SendFirebaseEevents("Bed_3_Unlock");
                break;
            case ShopItemType.bad4:
                //AdsManager.SendFirebaseEevents("Bed_4_Unlock");
                break;
        }
    }
    public void NotEnoughCurrency(CurrencyType currencyType)
    {
        notEnoughCurrencyLog.SetActive(true);
        Debug.Log("Not Enough Currency " + currencyType);
    }
    public void CloseClick()
    {
        AudioManager.Instance?.PlaySFX(SFX.Click);

        gameObject.SetActive(false);
    }
}
[Serializable]
public class ShopItem
{
    public ShopItemData shopItemData;
    public Sprite icon;
    public CurrencyType currencyType;
    public int currencyValue;
}