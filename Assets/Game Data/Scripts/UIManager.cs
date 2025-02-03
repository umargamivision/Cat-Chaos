using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ommy.Singleton;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Ommy.SaveData;
using Ommy.Audio;
using UnityEngine.Rendering.Universal;
public class UIManager : Singleton<UIManager>
{
    public CurrencyCollectionAnimation currencyCollectionAnimation;
    [Header("Panels")]
    public GameObject newObjectivePanel;
    public GameObject gamePlay;
    public GameObject completePanel, failPanel, pausePanel,settingPanel,shopPanel;

    [Header("Texts")]
    public TMP_Text levelNoTxt;
    public TMP_Text newObjectiveTxt;

    [Header("Images")]
    public Image levelFillImg;

    [Header("Buttons")]
    public GameObject grabButton;
    public Button doorButton, lampButton;
    public Button tearButton;
    public Button sleepButton;
    public Button grabSpecialItem;

    public void Pause(bool pause)
    {
        if(pause)
        {
            AdsManager.SendFirebaseEevents("Pause_Clk");
            AdsManager.ShowInterstitilAd("Inter_pause_btn");
        }

        Time.timeScale = pause? 0:1;
        AudioManager.Instance?.PlaySFX(SFX.Click);
        pausePanel.SetActive(pause);
    }
    public void SetNewObjective(string objective)
    {
        newObjectivePanel.SetActive(true);
        newObjectiveTxt.text = objective;
    }
    public void NoThanks2XClick()
    {
        AudioManager.Instance?.PlaySFX(SFX.Click);
        Time.timeScale=1;
        currencyCollectionAnimation.CollectCash();

        int fishs = GameManager.Instance.GetFishes();
        fishs=fishs+2;
    
        CurrencyManager.Instance.UpdateCurrency(fishs);
        completePanel.SetActive(false);
        if(GameManager.Instance.isGameCompleted)
        {
            GameManager.Instance.isGameCompleted = false;
            GameManager.Instance.LoadScene(GameManager.Instance.gameplayScene,0);
        }
    }
    public void Reward2XClick()
    {
        AdsManager.ShowRewardedAd(Reward2XSuccess,"Lvl_cmplt");
    }
    public void Reward2XSuccess()
    {
        AudioManager.Instance?.PlaySFX(SFX.Click);
        Time.timeScale=1;
        currencyCollectionAnimation.CollectCash();
        int fishs = GameManager.Instance.GetFishes();
        fishs=fishs+6;
        CurrencyManager.Instance.UpdateCurrency(fishs);
        completePanel.SetActive(false);
        if(GameManager.Instance.isGameCompleted)
        {
            GameManager.Instance.isGameCompleted = false;
            GameManager.Instance.LoadScene(GameManager.Instance.gameplayScene,0);
        }

    }
    public void LevelComplete()
    {
        AdsManager.ShowInterstitilAd("Inter_lvl_cmplt");
        Time.timeScale=0;
        completePanel.SetActive(true);
    }
    public void LevelFail()
    {
        failPanel.SetActive(true);
    }
    public void HomeClick()
    {
        Time.timeScale = 1;
        AudioManager.Instance?.PlaySFX(SFX.Click);
        GameManager.Instance.LoadScene(GameManager.Instance.mainMenuScene,3);
    }
    public void HintClick()
    {
        AudioManager.Instance?.PlaySFX(SFX.Click);
        AdsManager.ShowRewardedAd(OnHintSuccess,"Hint");
    }
    public void OnHintSuccess()
    {
        GameManager.SendLevelEvent("_Hint_Clk");
        //AdsManager.SendFirebaseEevents("Level_"+(1+LevelsManager.Instance.currentLevel)+"_Hint_Clk");
        GamePlayManager.Instance.ShowIndicators(true);
    }
    public void onCatBedDetected(CatBed catBed)
    {
        sleepButton.gameObject.SetActive(catBed!=null);
    }
    public void OnSpecialItemDetected(SpecialItem specialItem)
    {
        grabSpecialItem.gameObject.SetActive(specialItem!=null);
        if(specialItem==null) return;
        grabSpecialItem.onClick.RemoveAllListeners();
        grabSpecialItem.onClick.AddListener(specialItem.GrabClick);
    }
    public void OnSwitchDetect(ISwitch iSwitch)
    {
        if(iSwitch as Door)
        {
            doorButton.gameObject.SetActive(true);
            doorButton.onClick.RemoveAllListeners();
            doorButton.onClick.AddListener((iSwitch as Door).Toggle);
        }
        else if(iSwitch as LampController)
        {
            lampButton.gameObject.SetActive(true);
            lampButton.onClick.RemoveAllListeners();
            lampButton.onClick.AddListener((iSwitch as LampController).Toggle);
        }
        else
        {
            doorButton.gameObject.SetActive(false);
            lampButton.gameObject.SetActive(false);
        }
    }
    public void OnClothTearDetected(ClothTear cloth)
    {
        //tearButton.gameObject.SetActive(cloth!=null);
        // if(cloth==null) return;
        // tearButton.onClick.RemoveAllListeners();
        // tearButton.onClick.RemoveListener(cloth.TearIt);
        // tearButton.onClick.AddListener(cloth.TearIt);
    }
    public void SettingClick()
    {
        AudioManager.Instance?.PlaySFX(SFX.Click);
        settingPanel.SetActive(true);
    }
    public void SleepClick()
    {
        AudioManager.Instance?.PlaySFX(SFX.Click);
        GamePlayManager.Instance.SetCatSleep(true);
    }
    public void ShopClick()
    {
        AdsManager.SendFirebaseEevents("Pause_Shop_Clk");
        AudioManager.Instance?.PlaySFX(SFX.Click);
        shopPanel.SetActive(true);
    }
    public void UpdateLevelBar(int levelNo, float fillAmount)
    {
        levelNoTxt.text = $"Level "+(1+levelNo);
        levelFillImg.DOFillAmount(fillAmount,0.2f);
    }
    public void RestartClick()
    {
        AudioManager.Instance?.PlaySFX(SFX.Click);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ResetLevel()
    {

    }
}
