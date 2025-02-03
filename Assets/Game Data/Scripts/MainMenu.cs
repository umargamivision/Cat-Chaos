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
        AdsManager.SendFirebaseEevents("MainMenu");
        AdsManager.ShowBanner("Banner");
        Time.timeScale = 1;
        AudioManager.Instance?.StartGame();

        UpdateCurrency(GameManager.Instance.GetFishes());
        InvokeRepeating(nameof(UpdateCurrency), 0, 1);
    }
    public void PlayClick()
    {
        AdsManager.SendFirebaseEevents("PlayButton");
        AdsManager.ShowInterstitilAd("Inter_play_btn");
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
        shopPanel.SetActive(true);
        AudioManager.Instance?.PlaySFX(SFX.Click);
        AdsManager.SendFirebaseEevents("MainMenu_Shop_Clk");
    }
    public void UpdateCurrency()
    {
        UpdateCurrency(GameManager.Instance.GetFishes());
    }
    public void UpdateCurrency(int currency)
    {
        currencyText.text = currency.ToString();
    }
}
