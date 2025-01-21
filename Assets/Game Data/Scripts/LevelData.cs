using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Level", order = 1)]
public class LevelData : ScriptableObject
{
    public int levelNo;
    public int requireXP;
    public List<Task> tasks;
    public UnityEvent OnLevelComplete, OnTaskComplete;
    [Space(10)]
    [Header("Debug")]
    public bool hasCompleted;
    public float progress;
    public int currentXP;
    public void ResetLevel()
    {
        progress = 0;
        currentXP = 0;
        foreach (var task in tasks)
        {
            task.ResetValues();
        }
    }
    private void Reset() 
    {
        ResetLevel();    
    }
    private void OnEnable()
    {
        ResetLevel();
        foreach (var item in tasks)
        {
            item.OnComplete.AddListener(OnCompleteTask);
        }
    }
    private void OnDisable()
    {
        foreach (var task in tasks)
        {
            task.OnComplete.RemoveListener(OnCompleteTask);
        }
    }
    private void UpdateProgress()
    {
        if (tasks.Count == 0) return;

        int completedTasks = 0;
        foreach (var task in tasks)
        {
            if (task.complete) completedTasks++;
        }

        progress = (float)completedTasks / tasks.Count;
    }

    // Call this in OnCompleteTask
    public void OnCompleteTask(int xp)
    {
        currentXP += xp;
        XPManager.Instance.AddXP(xp); // Add XP to global pool
        UpdateProgress();
        OnTaskComplete.Invoke();
        if (currentXP >= requireXP)
        {
            hasCompleted = true;
            OnLevelComplete.Invoke();
        }
    }
}
