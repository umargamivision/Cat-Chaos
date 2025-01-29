using System.Collections;
using System.Collections.Generic;
using Ommy.Audio;
using Ommy.SaveData;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject settingPanel;
    public GameObject shopPanel;
    public TMP_Text currencyText;
    public void Start()
    {
        Time.timeScale = 1;
        AudioManager.Instance?.StartGame();

        UpdateCurrency(GameManager.Instance.Fishs);
    }
    public void PlayClick()
    {
        AudioManager.Instance?.PlaySFX(SFX.Click);
        GameManager.Instance.LoadScene(GameManager.Instance.gameplayScene, GameManager.Instance.loadingDely);
    }
    public void SettingClick()
    {
        AudioManager.Instance?.PlaySFX(SFX.Click);
        settingPanel.SetActive(true);
    }
    public void ShopClick()
    {
        AudioManager.Instance?.PlaySFX(SFX.Click);
        shopPanel.SetActive(true);
    }
    public void UpdateCurrency(int currency)
    {
        currencyText.text = currency.ToString();
    }
}
