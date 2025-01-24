using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
[Serializable]
public class LevelData
{
    [Serializable]
    public class TaskProp
    {
        public Task task;
        public List<Item> items;
    }
    //public TimelineType startTimeline,endTimeline;
    public PlayableDirector startDirector,endDirector;
    public int levelNo;
    public int requireXP;
    public List<TaskProp> tasks;
    public UnityEvent OnLevelComplete, OnTaskComplete;
    [Space(10)]
    [Header("Debug")]
    public bool hasCompleted;
    public float progress;
    public int currentXP;
    public void ResetLevel()
    {
        hasCompleted = false;
        progress = 0;
        currentXP = 0;
        foreach (var task in tasks)
        {
            task.task.ResetValues();
        }
    }
    public TaskProp CurrentTaskProp()
    {
        return tasks.Find(f=>!f.task.complete);
    }
    private void Reset() 
    {
        ResetLevel();    
    }
    public void SubscribeEvents()
    {
        ResetLevel();
        foreach (var item in tasks)
        {
            item.task.OnComplete.AddListener(OnCompleteTask);
        }
    }
    public void UnSubscribeEvents()
    {
        foreach (var task in tasks)
        {
            task.task.OnComplete.RemoveListener(OnCompleteTask);
        }
    }
    private void UpdateProgress()
    {
        if (tasks.Count == 0) return;

        int completedTasks = 0;
        foreach (var task in tasks)
        {
            if (task.task.complete) completedTasks++;
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
        if (progress >= 1)
        {
            hasCompleted = true;
            OnLevelComplete.Invoke();
        }
    }
}
