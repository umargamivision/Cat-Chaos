using System.Collections;
using System.Collections.Generic;
using Ommy.SaveData;
using UnityEngine;
using UnityEngine.Events;

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
        levelObjects[currentLevel].SetActive(true);
    }
    public void OnLevelComplete()
    {
        print("Level complete success");
    }
}
