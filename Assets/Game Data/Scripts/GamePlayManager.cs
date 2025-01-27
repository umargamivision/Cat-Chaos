using System;
using System.Collections;
using System.Collections.Generic;
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
    public UIManager uIManager;
    public TimelineManager timelineManager;
    private void Start()
    {
        Time.timeScale = 1;
    }
    public void SetupLevel(LevelData levelData)
    {
        questManager.Init(levelData);
        timelineManager.PlayTimeline(levelData.startDirector);
        uIManager.UpdateLevelBar(levelData.levelNo, levelData.progress);
    }
    public void OnCompleteLevel(LevelData levelData)
    {
        uIManager.LevelComplete();
        //timelineManager.PlayTimeline(levelData.endDirector);
    }
    public void OnTaskComplete(LevelData currentLevelData)
    {
        uIManager.UpdateLevelBar(currentLevelData.levelNo, currentLevelData.progress);
        uIManager.SetNewObjective(currentLevelData.CurrentTaskProp().task.discription);
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
        uIManager.LevelComplete();
        Debug.Log("game complete");
    }
    public void ShowIndicators(bool show)
    {
        levelsManager.ShowIndicators(show);
    }

    public void Sleep()
    {
        //timelineManager.PlayTimeline()
    }
}
