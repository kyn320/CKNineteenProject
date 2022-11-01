using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameTimeController : Singleton<GameTimeController>
{

    public UnityEvent<float> updateTimeScaleEvent;

    public void ChangeTimeScale(float timeScale)
    {
        updateTimeScaleEvent?.Invoke(timeScale);
        Time.timeScale = timeScale;
    }

}
