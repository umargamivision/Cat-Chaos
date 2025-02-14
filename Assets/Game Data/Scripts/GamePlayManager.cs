using System;
using System.Collections;
using System.Collections.Generic;
using Ommy.Audio;
using Ommy.SaveData;
using Ommy.Singleton;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;

public class GamePlayManager : Singleton<GamePlayManager>
{
    public event Action onComplete;
    public event Action onReset;
    public event Action onFail;

    public PlayerController playerController;
    public GrannyController grannyController;
    public QuestManager questManager;
    public LevelsManager levelsManager;
    public CatBed catBed;
    public UIManager uIManager;
    public TimelineManager timelineManager;
    public SpecialOffer specialOffer;
    private void Start()
    {
        Time.timeScale = 1;
    }
    private void Update() 
    {
        specialOffer.canShowOffer = CanShowOffer();    
    }
    public bool CanShowOffer()
    {
        var can = true;
        if(playerController.grabber.currentGrabbable!=null)
        {
            can = false;
        }
        if(playerController.vaccumCleaner.gameObject.activeInHierarchy)
        {
            can = false;
        }
        if(playerController.electricGun.gameObject.activeInHierarchy)
        {
            can = false;
        }
        if(playerController.fireSpray.gameObject.activeInHierarchy)
        {
            can = false;
        }
        if(uIManager.completePanel.activeInHierarchy)
        {
            can = false;
        }
        if(timelineManager.timelineCanvas.activeInHierarchy)
        {
            can = false;
        }
        return can;
    }
    public void AvailSpecialOffer(SpecialItemType specialItemType)
    {
        playerController.ActivateSpecialWeapon(specialItemType);
    }
    public void SetCatSleep(bool sleep)
    {
        catBed.Sleep(sleep);
    }
    public void ShootGranny(SpecialItemType specialItemType)
    {
        grannyController.ShootGranny(specialItemType);
    }
    public void SetupLevel(LevelData levelData)
    {
        if(SaveData.Instance.levelNoForEvent>=0)
        {
            specialOffer.RestartTimer();
        }
        questManager.Init(levelData);
        timelineManager.PlayTimeline(levelData.startDirector);
        //uIManager.UpdateLevelBar(levelData.levelNo, levelData.progress);
        uIManager.UpdateLevelBar(SaveData.Instance.levelNoForEvent, levelData.progress);
        GameManager.SendLevelEvent("_Play");
        //AdsManager.SendFirebaseEevents("Level_"+(1+levelsManager.currentLevel)+"_Play");
    }
    public void OnCompleteLevel(LevelData levelData)
    {
        uIManager.LevelComplete();
        GameManager.SendLevelEvent("_Complete");
        //AdsManager.SendFirebaseEevents("Level_"+(1+levelsManager.currentLevel)+"_Complete");
        //timelineManager.PlayTimeline(levelData.endDirector);
    }
    public void OnTaskComplete(LevelData currentLevelData)
    {
        AudioManager.Instance?.PlaySFX(SFX.TaskComplete);
        //uIManager.UpdateLevelBar(currentLevelData.levelNo, currentLevelData.progress);
        uIManager.UpdateLevelBar(SaveData.Instance.levelNoForEvent, currentLevelData.progress);
    }
    public void ResetData()
    {
        uIManager.ResetLevel();
        playerController.Respawn();
        levelsManager.ResetLevel();
        Debug.Log("data reseted");
    }
    public void LevelFail()
    {
        uIManager.LevelFail();
        Debug.Log("game fail");
    }
    public void LevelComplete()
    {
        AudioManager.Instance?.PlaySFX(SFX.LevelComplete);
        uIManager.LevelComplete();
        Debug.Log("game complete");
    }
    public void ShowIndicators(bool show)
    {
        levelsManager.ShowIndicators(show);
    }


}
