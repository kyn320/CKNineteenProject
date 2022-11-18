using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public const int MaxSFXPlayCount = 5;
    [SerializeField]
    private float bgmMasterVolume;
    public float BGMMasterVolume
    {
        get { return bgmMasterVolume; }
    }
    [SerializeField]
    private float sfxMasterVolume;
    public float SFXMasterVolume
    {
        get { return sfxMasterVolume; }
    }

    private AudioSource bgmAudioPlayer;
    [SerializeField]
    private float bgmVolume;

    [SerializeField]
    private SerializableDictionary<string, SoundPool> sfxPlayerDic = new SerializableDictionary<string, SoundPool>();
    [SerializeField]
    private GameObject sfxPlayerPrefab;

    protected override void Awake()
    {
        bgmAudioPlayer = GetComponent<AudioSource>();
        base.Awake();
    }

    public void ChangeBGMVolume(float bgm)
    {
        bgmMasterVolume = bgm;
        bgmAudioPlayer.volume = bgmVolume * bgmMasterVolume;
    }

    public void ChangeSFXVolume(float sfx)
    {
        sfxMasterVolume = sfx;

        var keyList = sfxPlayerDic.Keys.ToList();

        for (var i = 0; i < keyList.Count; ++i)
        {
            sfxPlayerDic[keyList[i]].ChangeMasterVolume(sfx);
        }
    }

    public void PlayBGM(AudioClip bgm, float volume)
    {
        bgmVolume = volume;

        bgmAudioPlayer.volume = bgmVolume * bgmMasterVolume;

        if (bgmAudioPlayer.clip != bgm)
        {
            bgmAudioPlayer.Stop();
            bgmAudioPlayer.clip = bgm;
            bgmAudioPlayer.Play();
        }

    }

    public void PauseBGM()
    {
        bgmAudioPlayer.Pause();
    }

    public void UnPauseBGM()
    {
        bgmAudioPlayer.UnPause();
    }

    public void StopBGM()
    {
        bgmAudioPlayer.Stop();
    }

    public void PlaySFX(AudioClip sfxClip, bool isLoop = false)
    {
        var audioPlayerObject = Instantiate(sfxPlayerPrefab, transform);
        var audioPlayer = audioPlayerObject.GetComponent<AudioPlayer>();

        if (!sfxPlayerDic.ContainsKey(sfxClip.name))
        {
            sfxPlayerDic.Add(sfxClip.name, new SoundPool());
        }

        var soundPool = sfxPlayerDic[sfxClip.name];
        soundPool.CheckPool();

        soundPool.AddAudioPlayer(audioPlayer);

        var audioSource = audioPlayer.AudioSource;
        audioSource.loop = isLoop;
        audioSource.clip = sfxClip;
        audioSource.Play();
    }

    public void PlaySFX(AudioPlayer audioPlayer, AudioClip sfxClip, bool isLoop = false)
    {
        if (!sfxPlayerDic.ContainsKey(sfxClip.name))
        {
            sfxPlayerDic.Add(sfxClip.name, new SoundPool());
        }

        var soundPool = sfxPlayerDic[sfxClip.name];
        soundPool.CheckPool();

        soundPool.AddAudioPlayer(audioPlayer);

        var audioSource = audioPlayer.AudioSource;
        audioSource.loop = isLoop;
        audioSource.clip = sfxClip;
        audioSource.Play();
    }

    public void RemoveSFXPlayer(AudioPlayer audioPlayer)
    {
        var key = audioPlayer.audioClipKey;

        if (string.IsNullOrEmpty(key))
            return;

        if (sfxPlayerDic.ContainsKey(key))
        {
            var soundPool = sfxPlayerDic[key];
            soundPool.RemoveAudioPlayer(audioPlayer);
        }
    }

}
