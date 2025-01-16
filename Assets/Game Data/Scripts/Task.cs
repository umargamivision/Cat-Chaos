using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewTask", menuName = "ScriptableObjects/Task", order = 1)]
public class Task : ScriptableObject
{
    public bool complete;
    public int countsForCompletion;
    public int taskCounts;
    public int xp;
    public string discription;
    public List<Task> prerequisites;
    public UnityEvent OnExecute;
    public UnityEvent<int> OnComplete;
    private void OnValidate()
    {
        if (countsForCompletion <= 0)
        {
            Debug.LogWarning($"[Task] '{name}' - countsForCompletion should be greater than 0.");
            countsForCompletion = 1; // Ensure a minimum value
        }

        if (string.IsNullOrEmpty(discription))
        {
            Debug.LogWarning($"[Task] '{name}' - Description is missing.");
        }
    }
    private void OnEnable()
    {
        ResetValues();
    }
    private void Reset()
    {
        ResetValues();
    }
    public void ResetValues()
    {
        complete = false;
        taskCounts = 0;
    }
    public bool CanExecute()
    {
        if(complete) return false;
        foreach (var prerequisite in prerequisites)
        {
            if (!prerequisite.complete)
            {
                return false;
            }
        }
        return true;
    }
    public void Execute()
    {
        if (!CanExecute())
        {
            Debug.LogWarning($"[Task] '{name}' cannot be executed. Prerequisites are incomplete.");
            return;
        }
        taskCounts++;
        OnExecute.Invoke();
        CompleteChecker();
    }
    public void CompleteChecker()
    {
        if (taskCounts >= countsForCompletion)
        {
            complete = true;
            OnComplete.Invoke(xp);
        }
    }
}
