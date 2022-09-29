using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISoundSettingView : MonoBehaviour
{
    SoundManager soundManager;

    public Slider bgmVolumeSlider;
    public Slider sfxVolumeSlider;

    private void Start()
    {
        soundManager = SoundManager.Instance;

        bgmVolumeSlider.value = soundManager.BGMMasterVolume;
        sfxVolumeSlider.value = soundManager.SFXMasterVolume;
    }

    public void UpdateBGM(float value)
    {
        soundManager.ChangeBGMVolume(value);
    }

    public void UpdateSFX(float value)
    {
        soundManager.ChangeSFXVolume(value);
    }

}
