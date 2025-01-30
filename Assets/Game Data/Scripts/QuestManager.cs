using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ommy.Singleton;
using UnityEngine;
using UnityEngine.Events;

public class QuestManager : Singleton<QuestManager>
{
    public List<QuestUI> mainQuestUIs;
    LevelData levelData;
    public void Init(LevelData _levelData)
    {
        levelData = _levelData;
        
        levelData.tasks.ForEach(f =>
        f.task.OnComplete.AddListener((int param) =>
        {
            UpdateQuest();

        }));
        UpdateQuest();
    }
    public void UpdateQuest()
    {
        //Task task = levelData.tasks.Find((f) => !f.complete);
        
        var LD = levelData.CurrentTaskProp();
        if(LD==null) return;
        Omar.Task task = LD.task;
        if(task == null) return;
        if (!task.complete) 
        {
            mainQuestUIs[0].UpdateTask(task,0.1f);
            LD.onStart.Invoke();
            UIManager.Instance.SetNewObjective(task.discription);
        }
    }
}
