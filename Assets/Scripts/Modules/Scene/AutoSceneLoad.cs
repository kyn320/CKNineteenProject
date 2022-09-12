using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSceneLoad : MonoBehaviour
{
    [SerializeField]
    private string sceneName;

    // Start is called before the first frame update
    void Start()
    {
        SceneLoader.Instance.SwitchScene(sceneName);        
    }

}
