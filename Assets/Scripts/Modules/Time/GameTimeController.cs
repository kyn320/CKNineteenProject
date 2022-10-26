using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameTimeController : Singleton<GameTimeController>
{
    public UnityEvent<float> updateTimeScaleEvent;

    private Coroutine timeScaleCoroutine;

    public void ChangeTimeScale(float timeScale)
    {
        updateTimeScaleEvent?.Invoke(timeScale);
        Time.timeScale = timeScale;
    }

    public void ChangeTimeScale(float timeScale, float lifeTime)
    {
        updateTimeScaleEvent?.Invoke(timeScale);
        Time.timeScale = timeScale;

        if (timeScaleCoroutine != null)
            StopCoroutine(timeScaleCoroutine);

        timeScaleCoroutine = StartCoroutine(CoTimeScaleAnimation(lifeTime));
    }

    IEnumerator CoTimeScaleAnimation(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        ChangeTimeScale(1f);
    }

}
