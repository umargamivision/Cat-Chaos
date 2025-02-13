using System;
using System.Collections;
using System.Collections.Generic;
using Ommy.SaveData;
using Ommy.Singleton;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class GameManager : Singleton<GameManager>
{

    [Header ("Preferences")]
    public bool isGameCompleted;
    [Header ("Scene Loading Data")]
    public string gameplayScene = "Chapter 1";
    public string mainMenuScene = "MainMenu";
    public int loadingDely;
    public Image loadingFill;
    public GameObject loadingCanvas;
    private void Start() 
    {
        SceneManager.LoadScene(mainMenuScene);    
    }
    private new void Awake()
    {
        base.Awake(); // Ensure Singleton initialization if needed.
        SaveSystem.LoadProgress();
        Application.targetFrameRate = 30;
    }
    public int GetKeys()
    {
        SaveSystem.LoadProgress();
        return SaveData.Instance.Keys;
    }
    public void SetKeys(int value)
    {
        SaveData.Instance.Keys = value;
        SaveSystem.SaveProgress();
    }
    public int GetFishes()
    {
        SaveSystem.LoadProgress();
        return SaveData.Instance.Fishs;
    }
    public void SetFishes(int value)
    {
        SaveData.Instance.Fishs = value;
        SaveSystem.SaveProgress();
    }
    public static void SendLevelEvent(string eventName)
    {
        var eventlevelNo = GetLevelNoForEvent();
        if(eventlevelNo>24)
        {
            return;
        }
        //AdsManager.SendFirebaseEevents("Level_"+(1+eventlevelNo)+eventName);
    }
    public static int GetLevelNoForEvent()
    {
        return SaveData.Instance.levelNoForEvent;
    }
    public void LoadScene(string name, float delay)
    {
        StartCoroutine(LoadSceneAsync(name, delay));
    }
    private IEnumerator LoadSceneAsync(string name, float delay)
    {
        //AdsManager.ShowMrec(name==mainMenuScene? "Mrec_ply_btn":"Mrec_go_home");
        loadingCanvas.SetActive(true);
        loadingFill.DOFillAmount(1, delay);
        yield return new WaitForSeconds(delay);
        yield return new WaitForSeconds(0.1f);
        var operation = SceneManager.LoadSceneAsync(name);
        while (!operation.isDone)
        {
            yield return null;
        }
        //AdsManager.HideMrec();
        loadingCanvas.SetActive(false);
    }
}
