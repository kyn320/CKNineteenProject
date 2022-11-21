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

    [SerializeField]
    private GameObject uiDamageText;

    public bool isDeath = false;
    public UnityEvent<float, float> updateHpEvent;
    public UnityEvent<bool, UnitStatus> updateDeathEvent;

    protected virtual void OnEnable()
    {
        SetOriginStatus();
    }

    protected virtual void Start()
    {
        var hpElement = currentStatus.GetElement(StatusType.HP);
        var maxHPElement = currentStatus.GetElement(StatusType.MaxHP);

        hpElement.updateAmountAction += (value) =>
        {
            updateHpEvent?.Invoke(hpElement.CalculateTotalAmount(), maxHPElement.CalculateTotalAmount());
        };

        maxHPElement.updateAmountAction += (value) =>
        {
            updateHpEvent?.Invoke(hpElement.CalculateTotalAmount(), maxHPElement.CalculateTotalAmount());
        };

        updateHpEvent?.Invoke(hpElement.CalculateTotalAmount(), maxHPElement.CalculateTotalAmount());
    }

    public void SetOriginStatus()
    {
        if (originStatusData != null)
            currentStatus.Copy(originStatusData.StausDic);
    }

    public void SetCurrentStatus(StatusInfo statusInfo)
    {
        currentStatus = statusInfo;
    }

    public bool GetCriticalSuccess()
    {
        var tryPercent = Random.Range(0f, 100f);
        return tryPercent <= currentStatus.GetElement(StatusType.CriticalPercent).CalculateTotalAmount();
    }


    [Button("µ¥¹ÌÁö")]
    public virtual DamageInfo OnDamage(DamageInfo damageInfo)
    {
        if (isDeath)
        {
            damageInfo.isHit = false;
            damageInfo.isKill = false;
            return damageInfo;
        }

        damageInfo.isHit = true;

        var hpElement = currentStatus.GetElement(StatusType.HP);
        var maxHPElement = currentStatus.GetElement(StatusType.MaxHP);

        var damageText = UIController.Instance.CreateWorldUI(uiDamageText);
        damageText.GetComponent<UIDamageText>().SetDamageAmount(damageInfo.hitPoint, damageInfo.damage);

        hpElement.SubAmount(damageInfo.damage);

        updateHpEvent?.Invoke(hpElement.CalculateTotalAmount(), maxHPElement.CalculateTotalAmount());

        if (hpElement.CalculateTotalAmount() <= 0)
        {
            isDeath = true;
            damageInfo.isKill = true;
            updateDeathEvent?.Invoke(isDeath, this);
        }

        return damageInfo;
    }

}
