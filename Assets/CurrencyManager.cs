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
        UpdateCurrency(SaveData.Instance.Fishs);
    }
    public void UpdateCurrency(int currency)
    {
        currencyText.text = currency.ToString();
    }
}
