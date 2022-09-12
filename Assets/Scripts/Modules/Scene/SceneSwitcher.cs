using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSwitcher : MonoBehaviour
{
    public void SwitchScene(string sceneName)
    {
        SceneLoader.Instance.SwitchScene(sceneName);
    }

}
