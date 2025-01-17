using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Ommy.Attribute;
public class QuestUI : MonoBehaviour
{
    [Header("Quest Data")]
    public TMP_Text discriptionTxt, xpTxt;
    public Task currentTask;
    [Header("Quest Complete Object")]
    public Image completeBG;
    public Transform completeTitle, completeTick;
    public void SubscribeEvents(Task task)
    {
        task.OnExecute.AddListener(UpdateUI);
        task.OnComplete.AddListener(OnQuestComplete);
    }
    public void UnSubscribeEvents(Task task)
    {
        task.OnExecute.RemoveListener(UpdateUI);
        task.OnComplete.RemoveListener(OnQuestComplete);
    }
    public void SetupTask(Task _task)
    {
        gameObject.SetActive(true);
        currentTask = _task;
        SubscribeEvents(currentTask);
        UpdateUI();
    }
    public void UpdateTask(Task task, float delay)
    {
        if(currentTask!=null) UnSubscribeEvents(currentTask);
        //PlayCompleteAnimation(true);
        if(task!=null)CoroutineManager.Instance.StartCoroutine(UpdateTaskDelay(task,delay));
    }
    public IEnumerator UpdateTaskDelay(Task _task, float delay)
    {
        yield return new WaitForSeconds(delay);
        PlayCompleteAnimation(false);
        SetupTask(_task);
    }
    public void UpdateUI()
    {
        xpTxt.text = $"{currentTask.xp} XP";
        discriptionTxt.text = $"{currentTask.discription} ({currentTask.taskCounts}/{currentTask.countsForCompletion})";
    }
    public void OnQuestComplete(int xp)
    {
        PlayCompleteAnimation(true);
    }
    [InspectorButton]
     void PlayCompleteAnimation(bool forward)
    {
        if (forward)
        {
            //Quest Objects
            xpTxt.DOFade(0,0.1f).SetAutoKill(false);
            discriptionTxt.DOFade(0,0.1f).SetAutoKill(false);
            //Complete Objects
            completeBG.DOFillAmount(1, 0.8f).From(0).SetAutoKill(false);//.PlayForward();
            completeTitle.DOScale(1, 0.8f).From(0).SetEase(Ease.OutBack).SetAutoKill(false);//.PlayForward();
            completeTick.DOScale(1, 0.8f).From(0).SetEase(Ease.OutQuad).SetAutoKill(false);//.PlayForward();
        }
        else
        {
            //Quest Objects
            xpTxt.DOPlayBackwards();
            discriptionTxt.DOPlayBackwards();
            //Complete Objects
            completeBG.DOPlayBackwards();
            completeTitle.DOPlayBackwards();
            completeTick.DOPlayBackwards();
        }
    }
}
