using System.Collections;
using System.Collections.Generic;
using Ommy.SaveData;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string gamePlaySceneName;
    public GameObject settingPanel;
    public GameObject shopPanel;
    public TMP_Text currencyText;
    public void Start()
    {
        UpdateCurrency(SaveData.Instance.Fishs);
    }
    public void PlayClick()
    {
        SceneManager.LoadScene(gamePlaySceneName);
    }
    public void SettingClick()
    {
        settingPanel.SetActive(true);
    }
    public void ShopClick()
    {
        shopPanel.SetActive(true);
    }
    public void UpdateCurrency(int currency)
    {
        currencyText.text = currency.ToString();
    }
}
