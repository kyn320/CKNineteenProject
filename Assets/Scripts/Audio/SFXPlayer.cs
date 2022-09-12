using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    public AudioClip audioClip;
    public SoundPoolPlayer sfxPlayer;

    private void Awake()
    {
        if(audioClip == null)
        {
            Debug.LogError("AudioClip is not Found");
        }
    }

    public void AudioPlay()
    {
        SoundManager.Instance.PlaySFX(audioClip, sfxPlayer);
    }

    public void AudioStop()
    {
        SoundManager.Instance.StopSFX(sfxPlayer);
    }



}
