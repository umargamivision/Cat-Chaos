using System;
using System.Collections;
using System.Collections.Generic;
using Ommy.FadeSystem;
using Ommy.Singleton;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class TimelineManager : Singleton<TimelineManager>
{
    public UnityEvent onPlay, onStop;
    public List<TimelineProp> timelineProps;
    public TimelineProp currentTL;
    public void PlayTimeline(TimelineType timelineType)
    {
        var TL = timelineProps.Find(f => f.timelineType == timelineType);
        if (TL != null)
        {
            currentTL = TL;

            TL.playableDirector.played += OnPlay;
            TL.playableDirector.paused += OnPause;
            TL.playableDirector.stopped += OnStop;

            Debug.Log(TL.timelineType + " start playing");
            currentTL.Play();
        }
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
        yield return new WaitForSeconds(1);
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
    timeline3
}