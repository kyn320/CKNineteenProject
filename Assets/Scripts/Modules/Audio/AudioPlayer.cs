using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

public abstract class AudioPlayer : MonoBehaviour
{
    [ReadOnly]
    [ShowInInspector]
    private Transform originParent;

    [ReadOnly]
    [ShowInInspector]
    private string useSceneName;

    [SerializeField]
    protected AudioSource audioSource;

    public AudioSource AudioSource
    {
        get { return audioSource; }
    }

    public string audioClipKey;

    [SerializeField]
    protected float volume = 0.5f;

    public bool aliveSceneChange = false;
    public bool autoPlay = true;
    public bool isLoop = false;
    public bool autoDestroy = false;


    [ReadOnly]
    [ShowInInspector]
    protected bool isPlay = false;

    public float Time
    {
        get
        {
            return audioSource == null ? 0f : audioSource.time;
        }
    }

    protected virtual void OnEnable()
    {
        if (audioSource == null)
        {
            audioSource = this.GetOrAddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }

        if (SoundManager.Instance != null && autoPlay)
        {
            Play();
        }
    }

    public virtual void Play()
    {
        if (aliveSceneChange)
        {
            useSceneName = SceneManager.GetActiveScene().name;
            originParent = transform.parent;
            transform.SetParent(SoundManager.Instance.transform);
        }

        isPlay = true;
    }

    protected virtual void Update()
    {
        if (isPlay && !audioSource.isPlaying)
        {
            isPlay = false;
            if (autoDestroy || (aliveSceneChange && useSceneName != SceneManager.GetActiveScene().name))
            {
                Destroy(this.gameObject);
            }
            else
            {
                if (aliveSceneChange)
                {
                    transform.SetParent(originParent);
                }

                SoundManager.Instance?.RemoveSFXPlayer(this);
            }
        }
    }

    public void Stop()
    {
        audioSource.Stop();
    }

    public void ChangeVolume(float changeVolume)
    {
        if (audioSource == null)
            return;

        audioSource.volume = changeVolume * SoundManager.Instance.SFXMasterVolume;
    }

    public void ChangeMasterVolume(float changeMasterVolume)
    {
        if(audioSource == null)
            return;

        audioSource.volume = volume * changeMasterVolume;
    }

    protected virtual void OnDisable()
    {
        Stop();
        SoundManager.Instance?.RemoveSFXPlayer(this);
    }
}
