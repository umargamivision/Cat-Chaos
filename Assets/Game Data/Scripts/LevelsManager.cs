using System.Collections;
using System.Collections.Generic;
using Ommy.SaveData;
using UnityEngine;
using System;

public class LevelsManager : MonoBehaviour
{
    public float loadNextLevelDelay;
    public int currentLevel => SaveData.Instance.Level;
    public List<GameObject> levelObjects;
    public List<LevelData> levelDatas;
    public LevelData currentLevelData => levelDatas[currentLevel];
    private void OnEnable() 
    {
        currentLevelData.OnLevelComplete.AddListener(OnLevelComplete);
        currentLevelData.OnTaskComplete.AddListener(OnTaskComplete);
        SetupLevel();
    }
    public void SetupLevel()
    {
        QuestManager.Instance.Init(currentLevelData);
        UIManager.Instance.UpdateLevelBar(currentLevelData.levelNo, currentLevelData.progress);
        foreach (var item in levelObjects)
        {
            item.SetActive(false);
        }
        levelObjects[currentLevel].SetActive(true);
    }
    public void OnLevelComplete()
    {
        SaveData.Instance.Level++;
        CoroutineManager.Instance.StartCor(NextLevel());
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
