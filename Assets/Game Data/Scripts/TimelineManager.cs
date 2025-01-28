using System;
using System.Collections;
using System.Collections.Generic;
using Ommy.FadeSystem;
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
    public Button skipButton;
    public UnityEvent onPlay, onStop;
    public List<TimelineProp> timelineProps;
    public TimelineProp currentTL;
    public DialogueManager dialogueManager;
    Action onDone;
    public void PlayTimeline(TimelineType timelineType, Action onDone = null)
    {
        var TL = timelineProps.Find(f => f.timelineType == timelineType);
        if (TL != null)
        {
            currentTL = TL;
            Play(onDone);
        }
    }
    public void UpdateDialogue()
    {
        if(currentDialogueIndex>=currentTL.dialogues.Length) return;
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
        currentTL.playableDirector.played += OnPlay;
        currentTL.playableDirector.paused += OnPause;
        currentTL.playableDirector.stopped += OnStop;

        Debug.Log(" start playing");

        this.onDone = onDone;

        currentTL.Play();
    }
    public void OnComplete()
    {
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
        if (currentTL != null)
        {
            StartCoroutine(SkipTimeline());
        }
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
    timeline1,
    timeline2,
    timeline3,
    PoopInFoodBox,
    sleepOnBed,
    sleepOnGrannyBed,
    grannyBringPigeon,
    grannyWatchingTV,
}