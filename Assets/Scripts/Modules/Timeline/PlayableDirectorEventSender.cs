using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class PlayableDirectorEventSender : MonoBehaviour
{
    private PlayableDirector playableDirector;

    public UnityEvent playEvent;
    public UnityEvent pauseEvent;
    public UnityEvent stopEvent;
    public UnityEvent endEvent;

    public bool autoPlayOnStart = true;
    private bool isPlay = false;

    public void Awake()
    {
        playableDirector = GetComponent<PlayableDirector>();

        playableDirector.played += OnPlay;
        playableDirector.paused += OnPlay;
        playableDirector.stopped += OnStop;
    }

    private void Start()
    {
        if(autoPlayOnStart)
            playableDirector.Play();
    }

    private void Update()
    {
        if (playableDirector == null || !isPlay)
            return;

        if (playableDirector.time >= playableDirector.duration)
        {
            isPlay = false;
            endEvent.Invoke();
        }
    }

    protected void OnPlay(PlayableDirector pd)
    {
        isPlay = true;
        playEvent?.Invoke();
    }

    protected void OnPause(PlayableDirector pd)
    {
        pauseEvent?.Invoke();
    }

    protected void OnStop(PlayableDirector pd)
    {
        stopEvent?.Invoke();
    }


}
