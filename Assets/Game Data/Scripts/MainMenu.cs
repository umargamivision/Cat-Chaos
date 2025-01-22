using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string gamePlaySceneName;
    public GameObject settingPanel;
    public GameObject shopPanel;
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
}
