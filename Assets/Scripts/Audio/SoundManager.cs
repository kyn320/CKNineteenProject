using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    private AudioSource bgmPlayer;
    private SoundPool soundPool;

    public override void Awake()
    {
        if (bgmPlayer == null)
        {
            bgmPlayer = GetComponent<AudioSource>();

            if (bgmPlayer == null)
            {
                Debug.LogError("bgmPlayer is Not Found");
            }
        }

        if(soundPool == null)
        {
            soundPool = GetComponent<SoundPool>();

            if(soundPool == null)
            {
                Debug.Log("soundPool is Not Found");
            }
        }
    }

    public void PlayBGM(AudioClip audio)
    {
        bgmPlayer.clip = audio;
        bgmPlayer.Play();
    }

    public void StopBGM()
    {
        bgmPlayer.Stop();
    }

    public void PlaySFX(AudioClip audio, SoundPoolPlayer player)
    {
        Debug.Log(player.name);
        soundPool.Check(audio, player);
    }

    public void StopSFX(SoundPoolPlayer player)
    {
        soundPool.Stop(player);
    }


}