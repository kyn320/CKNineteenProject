using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Cysharp.Threading.Tasks;

public class GameInitializer : MonoBehaviour
{
    public string nextScene;

    private async void Start()
    {
        Application.targetFrameRate = 60;

        var saveData = SaveLoadSystem.Instance.SaveLoadData;

        var bgmVolume = saveData.bgmVolume;
        var sfxVolume = saveData.sfxVolume;

        SoundManager.Instance.ChangeBGMVolume(bgmVolume);
        SoundManager.Instance.ChangeSFXVolume(sfxVolume);

        GlobalSetting.Instance.mouseSensitivity = saveData.mouseSensitivity;

        await UniTask.Delay(100);
        SceneLoader.Instance.LoadScene(nextScene);
    }
}
