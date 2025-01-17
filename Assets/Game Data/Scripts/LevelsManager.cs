using System.Collections;
using System.Collections.Generic;
using Ommy.SaveData;
using UnityEngine;
using System;

public class LevelsManager : MonoBehaviour
{
    public int currentLevel => SaveData.Instance.Level;
    public List<GameObject> levelObjects;
    public List<LevelData> levelDatas;
    public LevelData currentLevelData => levelDatas[currentLevel];
    private void OnEnable() 
    {
        currentLevelData.OnLevelComplete.AddListener(OnLevelComplete);
        SetupLevel();
    }
    public void SetupLevel()
    {
        QuestManager.Instance.Init(currentLevelData);
        foreach (var item in levelObjects)
        {
            item.SetActive(false);
        }
        levelObjects[currentLevel].SetActive(true);
    }
    public void OnLevelComplete()
    {
        SaveData.Instance.Level++;
        SetupLevel();
        print("Level complete success");
    }
}
