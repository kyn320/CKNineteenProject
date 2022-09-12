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
        await UniTask.Delay(100);
        SceneLoader.Instance.LoadScene(nextScene);
    }
}
