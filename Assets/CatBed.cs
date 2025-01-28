using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Ommy.FadeSystem;
using Ommy.SaveData;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.UI;

public class CatBed : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public Image fadeImg;
    public float sleepTime;
    public Transform cat;
    public Transform awakeUpPos;
    public UnityEvent onSleep;
    public void Sleep(bool sleep)
    {
        if (sleep)
        {
            StartCoroutine(SleepTime());
            TimelineManager.Instance.PlayTimeline(playableDirector, () =>
            {
                onSleep.Invoke();
            });
        }
    }
    IEnumerator SleepTime()
    {
        yield return new WaitForSeconds(2);
        yield return fadeImg.DOFade(1, 1).WaitForCompletion();
        cat.SetPositionAndRotation(awakeUpPos.position,awakeUpPos.rotation);
        yield return new WaitForSeconds(sleepTime);
        yield return fadeImg.DOFade(0, 1).WaitForCompletion();
    }
    [Header("Visuals")]
    public Material bedMat;
    public Color[] bedColor;
    private void Start()
    {
        SetupBed();
    }
    public void SetupBed()
    {
        SaveSystem.LoadProgress();
        var SBType = SaveData.Instance.shopItemDatas.Find(f => f.hasSelected == true);
        bedMat.color = bedColor[(int)SBType.shopItemType];
    }
}
