using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AutoFadeOut : MonoBehaviour
{
    public UnityEvent endFadeEvent;

    private void Start()
    {
        FadeController.Instance.FadeOut(() => {
            endFadeEvent?.Invoke();
        });
    }
}
