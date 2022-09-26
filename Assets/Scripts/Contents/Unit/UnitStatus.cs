using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class UnitStatus : SerializedMonoBehaviour
{
    [SerializeField]
    protected StatusInfoData originStatusData;
    [SerializeField]
    public StatusInfo currentStatus;

    public bool isDeath = false;
    public UnityEvent<float, float> updateHpEvent;
    public UnityEvent<bool, UnitStatus> updateDeathEvent;

    private void OnEnable()
    {
        SetOriginStatus();
    }

    public void SetOriginStatus()
    {
        if (originStatusData != null)
            currentStatus.Copy(originStatusData.StausDic);
    }

    public void SetCurrentStatus(StatusInfo statusInfo) {
        currentStatus = statusInfo;
    }

    [Button("µ¥¹ÌÁö")]
    public virtual bool OnDamage(float damage)
    {
        if (isDeath)
            return false;

        var hpElement = currentStatus.GetElement(StatusType.HP);
        var maxHPElement = currentStatus.GetElement(StatusType.MaxHP);

        hpElement.SubAmount(damage);

        updateHpEvent?.Invoke(hpElement.GetAmount(), maxHPElement.GetAmount());

        if (hpElement.GetAmount() <= 0)
        {
            isDeath = true;
            updateDeathEvent?.Invoke(isDeath, this);
        }

        return isDeath;
    }

}
