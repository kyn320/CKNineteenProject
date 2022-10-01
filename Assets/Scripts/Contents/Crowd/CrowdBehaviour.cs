using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CrowdBehaviour : MonoBehaviour
{
    [SerializeField]
    private CrowdActionData actionData;

    [SerializeField]
    private float currentTime;
    [SerializeField]
    private float maxTime;

    [SerializeField]
    private bool isUpdate = false;

    public UnityEvent startEvent;
    public UnityEvent<float, float> updateTimerEvent;
    public UnityEvent endEvent;

    public void SetCrowdActionData(CrowdActionData actionData)
    {
        this.actionData = actionData;
    }

    public CrowdActionData GetCrowdActionData()
    {
        return actionData;
    }

    public virtual void Active()
    {
        isUpdate = true;
        ApplyCrowd();
        startEvent?.Invoke();
    }

    public virtual void UnActive()
    {
        isUpdate = false;
    }

    public virtual void Update()
    {
        if (!isUpdate)
            return;

        currentTime -= Time.deltaTime;

        updateTimerEvent?.Invoke(currentTime, maxTime);

        if (currentTime <= 0f)
        {
            isUpdate = false;
            endEvent?.Invoke();
        }
    }

    protected virtual void ApplyCrowd()
    {

    }

}
