using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    public ShopItem shopItem;
    public Sprite fish, key, ad;
    public Image iconImg, currencyImg;
    public TMP_Text currencyTxt;
    public GameObject curracnyBG;
    public void Setup(ShopItem _shopItem)
    {
        shopItem = _shopItem;
        currencyTxt.text = shopItem.currencyValue.ToString();
        iconImg.sprite = shopItem.icon;
        switch (shopItem.currencyType)
        {
            case CurrencyType.fish:
                currencyImg.sprite = fish;
                break;
            case CurrencyType.key:
                currencyImg.sprite = key;
                break;
            case CurrencyType.ad:
                currencyImg.sprite = ad;
                break;
        }
        if(shopItem.shopItemData.hasUnlocked) OnUnlocked();
    }
    public void Hightlight(bool select)
    {
        //shopItem.shopItemData.hasSelected = select;
        transform.GetChild(0).gameObject.SetActive(select);
    }
    public void OnUnlocked()
    {
        curracnyBG.gameObject.SetActive(false);
    }
    public void OnClick()
    {
        ShopManager.Instance.OnShopItemClick(this);
    }
}
