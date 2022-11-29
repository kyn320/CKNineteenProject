using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class RandomTimer : MonoBehaviour
{
    private float currentTime;
    public AmountRangeFloat timeRange;
    public bool isUpdate = true;
    public UnityEvent timeEndEvent;

    private void Start()
    {
        currentTime = timeRange.GetRandomAmount();
    }

    private void Update()
    {
        if (!isUpdate)
            return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = timeRange.GetRandomAmount();
            timeEndEvent?.Invoke();
        }
    }

}
