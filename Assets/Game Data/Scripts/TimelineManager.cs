using System;
using System.Collections;
using System.Collections.Generic;
using Ommy.Audio;
using Ommy.FadeSystem;
using Ommy.SaveData;
using Ommy.Singleton;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class TimelineManager : Singleton<TimelineManager>
{
    public int currentDialogueIndex;
    public bool isPlaying;
    public GameObject timelineCanvas;
    public Button skipButton;
    public GameObject skipAdIcon;
    public UnityEvent onPlay, onStop, onComplete;
    public List<TimelineProp> timelineProps;
    public TimelineProp currentTL;
    public DialogueManager dialogueManager;
    Action onDone;
    public void PlayTimeline(TimelineType timelineType, Action onDone = null)
    {
        CanShowAdOnSkip();
        var TL = timelineProps.Find(f => f.timelineType == timelineType);
        if (TL != null)
        {
            currentTL = TL;
            Play(onDone);
        }
    }
    public void UpdateDialogue()
    {
        if (currentDialogueIndex >= currentTL.dialogues.Length) return;
        dialogueManager.SetDialogue(currentTL.dialogues[currentDialogueIndex]);
        currentDialogueIndex++;
    }
    public void PlayTimeline(PlayableDirector playableDirector, Action onDone = null)
    {
        if (playableDirector == null) return;
        currentTL.playableDirector = playableDirector;
        Play(onDone);
    }
    void Play(Action onDone)
    {
        currentDialogueIndex = 0;
        currentTL.playableDirector.played += OnPlay;
        currentTL.playableDirector.paused += OnPause;
        currentTL.playableDirector.stopped += OnStop;

        Debug.Log(" start playing");

        this.onDone = onDone;
        onComplete.Invoke();

        currentTL.Play();
    }
    public void OnComplete()
    {
        onComplete.Invoke();
        onDone?.Invoke();
    }
    public void StopTimeLine()
    {
        if (currentTL != null)
        {
            currentTL.playableDirector.Stop();
            //currentTL.playableDirector.Evaluate();
        }
    }
    public void SkipTimelineClick()
    {
        AudioManager.Instance?.PlaySFX(SFX.Click);
        if(CanShowAdOnSkip())
            AdsManager.ShowRewardedAd(OnSkipTimeline, "Cutscene_skip");
        else
            OnSkipTimeline();
    }
    bool CanShowAdOnSkip()
    {
        if (SaveData.Instance.Level == 0)
        {
            skipAdIcon.SetActive(false);
            return false;
        }
        skipAdIcon.SetActive(true);
        return true;
    }
    public void OnSkipTimeline()
    {
        if (currentTL != null)
        {
            StartCoroutine(SkipTimeline());
        }

        if (SaveData.Instance.levelNoForEvent%8 == 0)
        {
            if (currentTL.timelineType == TimelineType.grannyBeatMom)
            {
                GameManager.SendLevelEvent("_Cutscene_1_Skip");
            }
            else if (currentTL.timelineType == TimelineType.grannyGotAngry)
            {
                GameManager.SendLevelEvent("_Cutscene_2_Skip");
            }
        }
        else if (SaveData.Instance.levelNoForEvent%8 == 2)
        {
            if (currentTL.timelineType == TimelineType.grannyWatchingTV)
            {
                GameManager.SendLevelEvent("_Cutscene_1_Skip");
            }
            else if (currentTL.timelineType == TimelineType.grannySlip)
            {
                GameManager.SendLevelEvent("_Cutscene_2_Skip");
            }
        }
        else if (SaveData.Instance.levelNoForEvent%8 == 5)
        {
            if (currentTL.timelineType == TimelineType.PoopInFoodBox)
            {
                GameManager.SendLevelEvent("_Cutscene_1_Skip");
            }
            else if (currentTL.timelineType == TimelineType.sleepOnGrannyBed)
            {
                GameManager.SendLevelEvent("_Cutscene_2_Skip");
            }
        }
        else
        {
            GameManager.SendLevelEvent("_Cutscene_Skip");
        }
        //AdsManager.SendFirebaseEevents("Level_"+(1+LevelsManager.Instance.currentLevel)+"_Cutscene_Skip");

    }
    IEnumerator SkipTimeline()
    {
        yield return ScreenFader.Instance.FadeOut();
        currentTL.playableDirector.time = currentTL.playableDirector.duration;
        yield return new WaitForSecondsRealtime(1);
        yield return ScreenFader.Instance.FadeIn();
    }
    public void OnPause(PlayableDirector playableDirector)
    {

    }
    public void OnPlay(PlayableDirector playableDirector)
    {
        onPlay.Invoke();
    }
    public void OnStop(PlayableDirector playableDirector)
    {
        onStop.Invoke();
    }
    [Serializable]
    public class TimelineProp
    {
        public string[] dialogues;
        public TimelineType timelineType;
        public PlayableDirector playableDirector;
        [InspectorButton]
        public void Play()
        {
            playableDirector.Play();
        }
    }
}
public enum TimelineType
{
    none,
    grannyBeatMom,
    grannyGotAngry,
    grannySlip,
    PoopInFoodBox,
    sleepOnBed,
    sleepOnGrannyBed,
    grannyBringPigeon,
    grannyWatchingTV,
}