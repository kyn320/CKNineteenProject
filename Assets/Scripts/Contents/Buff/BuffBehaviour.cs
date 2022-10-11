using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class BuffBehaviour : MonoBehaviour
{
    [SerializeField]
    private BuffController controller;

    [SerializeField]
    private BuffData buffData;

    [SerializeField]
    private StatusInfo status;

    [SerializeField]
    private float currentLifeTime;

    [SerializeField]
    private bool isAlive = false;

    [ShowInInspector]
    [ReadOnly]
    private int stackCount = 0;

    [SerializeField]
    private List<CrowdBehaviour> crowdBehaviours;

    public UnityEvent<BuffBehaviour> startBuffEvent;
    public UnityEvent<BuffBehaviour, float> updateBuffEvent;
    public UnityEvent<BuffBehaviour> endBuffEvent;

    public bool EqualBuffData(BuffData buffData)
    {
        return this.buffData == buffData;
    }

    public virtual void SetBuffController(BuffController controller)
    {
        this.controller = controller;
    }

    public virtual void SetBuffData(BuffData buffData)
    {
        this.buffData = buffData;
    }

    public virtual void Initialized()
    {
        if (buffData == null)
            return;

        status.Copy(buffData.StausDic);
    }

    public virtual void StartBuff()
    {
        Initialized();

        isAlive = true;
        ResetLifeTime();

        var targetStatus = controller.GetStatus();
        targetStatus.currentStatus.AddStatusInfo(status);

        startBuffEvent?.Invoke(this);
        updateBuffEvent?.Invoke(this, currentLifeTime);
    }

    protected virtual void Update()
    {
        if (!isAlive)
            return;

        currentLifeTime -= Time.deltaTime;
        updateBuffEvent?.Invoke(this, currentLifeTime);

        if (currentLifeTime <= 0f)
        {
            EndBuff();
        }
    }

    public virtual void EndBuff()
    {
        isAlive = false;

        var targetStatus = controller.GetStatus();
        targetStatus.currentStatus.SubStatusInfo(status);

        controller.RemoveBuffList(this);
        endBuffEvent?.Invoke(this);
    }

    public void AddStack()
    {
        ++stackCount;
        if (buffData.IsOverlap)
        {
            ResetLifeTime();
        }
        else
        {
            AddLifeTime();
        }
    }

    public virtual void AddLifeTime()
    {
        currentLifeTime += buffData.LifeTime;
    }
    public virtual void AddLifeTime(float addLifeTime)
    {
        currentLifeTime += addLifeTime;
    }

    public virtual void ResetLifeTime()
    {
        currentLifeTime = buffData.LifeTime;
    }
}


