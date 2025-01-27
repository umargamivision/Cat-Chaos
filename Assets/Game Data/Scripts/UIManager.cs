using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ommy.Singleton;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
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

    public void Pause(bool pause)
    {
        Time.timeScale = pause? 0:1;
        pausePanel.SetActive(pause);
    }
    public void SetNewObjective(string objective)
    {
        newObjectivePanel.SetActive(true);
        newObjectiveTxt.text = objective;
    }
    public void NoThanks2XClick()
    {
        Time.timeScale=1;
        //currencyCollectionAnimation.AnimateCash();
        completePanel.SetActive(false);
    }
    public void Reward2XClick()
    {
        Time.timeScale=1;
        currencyCollectionAnimation.CollectCash();
        completePanel.SetActive(false);

    }
    public void LevelComplete()
    {
        Time.timeScale=0;
        completePanel.SetActive(true);
    }
    public void LevelFail()
    {
        failPanel.SetActive(true);
    }
    public void HomeClick()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void HintClick()
    {
        GamePlayManager.Instance.ShowIndicators(true);
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
        tearButton.gameObject.SetActive(cloth!=null);
        if(cloth==null) return;
        tearButton.onClick.RemoveAllListeners();
        tearButton.onClick.AddListener(cloth.TearIt);
    }
    public void SettingClick()
    {
        settingPanel.SetActive(true);
    }
    public void SleepClick()
    {
        GamePlayManager.Instance.Sleep();
    }
    public void ShopClick()
    {
        shopPanel.SetActive(true);
    }
    public void UpdateLevelBar(int levelNo, float fillAmount)
    {
        levelNoTxt.text = $"Level "+levelNo;
        levelFillImg.DOFillAmount(fillAmount,0.2f);
    }
    public void RestartClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ResetLevel()
    {

    }
}
