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
    [Header("Panels")]
    public GameObject gamePlay;
    public GameObject completePanel, failPanel, pausePanel,settingPanel,shopPanel;

    [Header("Texts")]
    public TMP_Text levelNoTxt;

    [Header("Images")]
    public Image levelFillImg;

    [Header("Buttons")]
    public GameObject grabButton;
    public void Pause(bool pause)
    {
        Time.timeScale = pause? 0:1;
        pausePanel.SetActive(pause);
    }
    public void LevelComplete()
    {
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

    }
    public void SettingClick()
    {
        settingPanel.SetActive(true);
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
