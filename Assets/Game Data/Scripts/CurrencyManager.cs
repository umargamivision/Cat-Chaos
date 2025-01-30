using System.Collections;
using System.Collections.Generic;
using Ommy.SaveData;
using Ommy.Singleton;
using TMPro;
using UnityEngine;

public class CurrencyManager : Singleton<CurrencyManager>
{
    public int levelCompleteReward = 2;
    public TMP_Text currencyText;
    private void OnEnable() 
    {
        UpdateCurrency(GameManager.Instance.Fishs);
    }
    public void UpdateCurrency(int currency)
    {
        GameManager.Instance.Fishs = currency;
        currencyText.text = currency.ToString();
    }
}
