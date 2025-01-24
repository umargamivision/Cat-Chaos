using System.Collections;
using System.Collections.Generic;
using Ommy.SaveData;
using UnityEngine;
using System;

public class LevelsManager : MonoBehaviour
{
    public float loadNextLevelDelay;
    //public int currentLevel => SaveData.Instance.Level;
    public int currentLevel;
    public List<GameObject> levelObjects;
    public List<LevelData> levelDatas;
    //public LevelData currentLevelData => levelDatas[currentLevel];
    public LevelData currentLevelData;
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
        foreach (var item in levelObjects)
        {
            item.SetActive(false);
        }
        ResetLevel();
        levelObjects[currentLevel].SetActive(true);        
    }
    public void OnLevelComplete()
    {
        UpdateCurrentLevel();
        CoroutineManager.Instance.StartCor(NextLevel());
    }
    public void UpdateCurrentLevel()
    {
        SaveData.Instance.Level++;
        SaveSystem.SaveProgress();
        currentLevel = SaveData.Instance.Level;
        currentLevelData = levelDatas[currentLevel];
    }
    public void ShowIndicators(bool show)
    {
        currentLevelData.CurrentTaskProp().items.ForEach(f=>f.ShowIndicator(show));
    }
    public void OnTaskComplete()
    {
        UIManager.Instance.UpdateLevelBar(currentLevelData.levelNo, currentLevelData.progress);
    }
    public IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(loadNextLevelDelay);
        SetupLevel();
        print("Level complete success");
    }
}
