using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSFXPlayer : AudioPlayer
{
    [SerializeField]
    private List<AudioClip> audioClipList = new List<AudioClip>();

    private AudioClip GetRandomClip()
    {
        return audioClipList[Random.Range(0, audioClipList.Count)];
    }

    public override void Play()
    {
        base.Play();

        var audioClip = GetRandomClip();
        audioClipKey = audioClip.name;
        SoundManager.Instance.PlaySFX(this, audioClip, isLoop);
    }
}
