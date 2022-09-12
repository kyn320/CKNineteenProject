using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    public AudioClip audioClip;

    private void Awake()
    {
        if (audioClip == null)
        {
            Debug.LogError("AudioClip is not Found");
        }
    }

    public void AudioPlay()
    {
        SoundManager.Instance.PlayBGM(audioClip);
    }

    public void AudioStop()
    {
        SoundManager.Instance.StopBGM();
    }
}
