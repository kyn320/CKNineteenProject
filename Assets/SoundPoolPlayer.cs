using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPoolPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioPlayer;


    private void Awake()
    {
        if (audioPlayer == null)
        {
            audioPlayer = GetComponent<AudioSource>();
            if (audioPlayer == null)
            {
                Debug.LogError(gameObject.name + "audioPlayer is Not Found");
            }
        }
    }
    public void Play(AudioClip clip)
    {
        audioPlayer.clip = clip;
        audioPlayer.Play();
    }

    public float Time()
    {
        return audioPlayer.time;
    }
    public void Stop()
    {
        audioPlayer.Stop();
    }
}
