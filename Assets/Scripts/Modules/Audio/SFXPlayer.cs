using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : AudioPlayer
{
    [SerializeField]
    protected AudioClip audioClip;

    public override void Play()
    {
        base.Play();
        audioClipKey = audioClip.name;
        SoundManager.Instance.PlaySFX(this, audioClip, isLoop);
    }
}
