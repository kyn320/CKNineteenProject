using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class SceneLoader : Singleton<SceneLoader>
{
    [SerializeField]
    private string loadingSceneName;
    [SerializeField]
    private string nextSceneName;

    public void SwitchScene(string nextScene)
    {
        nextSceneName = nextScene;
        FadeController.Instance.FadeIn(() =>
        {
            LoadScene(loadingSceneName);
        });
    }

    public void SwitchDirectScene(string nextScene)
    {
        FadeController.Instance.FadeIn(() =>
        {
            LoadScene(nextScene);
        });
    }

    public void LoadNextScene()
    {
        LoadScene(nextSceneName);
        nextSceneName = "";
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void LoadScene(string sceneName, LoadSceneMode loadMode = LoadSceneMode.Single, UnityAction endAction = null)
    {
        SceneManager.LoadScene(sceneName, loadMode);
        endAction?.Invoke();
    }

    public async Task LoadSceneAsync(string sceneName, LoadSceneMode loadMode = LoadSceneMode.Single, bool allowActive = true, UnityAction<float> loadAction = null, UnityAction endAction = null)
    {
        var asyncOper = SceneManager.LoadSceneAsync(sceneName, loadMode);
        asyncOper.allowSceneActivation = allowActive;

        do
        {
            loadAction?.Invoke(asyncOper.progress);
            await UniTask.Yield(PlayerLoopTiming.Update);
        } while (asyncOper.isDone);

        endAction?.Invoke();
    }


}
