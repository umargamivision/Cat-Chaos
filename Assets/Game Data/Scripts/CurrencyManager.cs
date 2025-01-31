using System.Collections;
using System.Collections.Generic;
using Ommy.SaveData;
using Ommy.Singleton;
using TMPro;
using UnityEngine;

public class CurrencyManager : Singleton<CurrencyManager>
{
    public TMP_Text currencyText;
    private void OnEnable() 
    {
        UpdateCurrency(GameManager.Instance.GetFishes());
    }
    public void UpdateCurrency()
    {
        UpdateCurrency(GameManager.Instance.GetFishes());
    }
    public void UpdateCurrency(int currency)
    {
        GameManager.Instance.SetFishes(currency);
        currencyText.text = currency.ToString();
    }
}
