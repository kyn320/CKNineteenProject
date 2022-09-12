using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioClip audioClip;

    [SerializeField]
    protected float volume = 0.5f;

    public bool autoPlay = true;

    private void Start()
    {
        if (autoPlay)
            SoundManager.Instance.PlayBGM(audioClip, volume);
    }

    public void Play()
    {
        SoundManager.Instance.PlayBGM(audioClip, volume);
    }

    public void Pause()
    {
        SoundManager.Instance.PauseBGM();
    }

    public void UnPause()
    {
        SoundManager.Instance.UnPauseBGM();
    }

    public void Stop()
    {
        SoundManager.Instance.StopBGM();
    }

}
