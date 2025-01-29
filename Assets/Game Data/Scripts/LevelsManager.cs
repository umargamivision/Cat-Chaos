using System.Collections;
using System.Collections.Generic;
using Ommy.SaveData;
using UnityEngine;
using System;
using UnityEngine.Events;

public class LevelsManager : MonoBehaviour
{
    [Range(0,8)]
    public int totalLevels=8;
    public float loadNextLevelDelay;
    //public int currentLevel => SaveData.Instance.Level;
    public int currentLevel;
    public List<GameObject> levelObjects;
    public List<LevelData> levelDatas;
    //public LevelData currentLevelData => levelDatas[currentLevel];
    public LevelData currentLevelData;
    public UnityEvent onCompleteAllLevels;
    private void OnEnable() 
    {
        SetupLevel();
    }
    private void OnDisable() 
    {
        currentLevelData.UnSubscribeEvents();
    }
    public void ResetLevel()
    {
        currentLevelData.ResetLevel();
    }
    public void SetupLevel()
    {
        currentLevelData.UnSubscribeEvents();
        currentLevel = SaveData.Instance.Level;
        currentLevelData = levelDatas[currentLevel];
        currentLevelData.SubscribeEvents();
        currentLevelData.OnLevelComplete.AddListener(OnLevelComplete);
        currentLevelData.OnTaskComplete.AddListener(OnTaskComplete);

        GamePlayManager.Instance.SetupLevel(currentLevelData);
        levelDatas.ForEach(f=>f.tasks.ForEach(i=>i.SetIndicators(false)));
        foreach (var item in levelObjects)
        {
            item.SetActive(false);
        }
        ResetLevel();
        levelObjects[currentLevel].SetActive(true);        
    }
    public void OnLevelComplete()
    {
        GamePlayManager.Instance.OnCompleteLevel(currentLevelData);
        UpdateCurrentLevel();
        CoroutineManager.Instance.StartCor(NextLevel());
    }
    public void UpdateCurrentLevel()
    {
        SaveData.Instance.Level++;
        if(SaveData.Instance.Level>=totalLevels)
        {
            SaveData.Instance.Level=0;
            onCompleteAllLevels.Invoke();
        }
        SaveSystem.SaveProgress();
        currentLevel = SaveData.Instance.Level;
        currentLevelData = levelDatas[currentLevel] ;
    }
    public void ShowIndicators(bool show)
    {
        currentLevelData.CurrentTaskProp().indicators.ForEach(f=>f.gameObject.SetActive(show));
    }
    public void OnTaskComplete()
    {
        GamePlayManager.Instance.OnTaskComplete(currentLevelData);
    }
    public IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(loadNextLevelDelay);
        SetupLevel();
        print("Level complete success");
    }
}
