using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

[System.Serializable]
public class StatusElement
{
    public StatusType type;
    public string name;
    [SerializeField]
    protected float amount;
    [SerializeField]
    protected float percent;

    public UnityAction<float> updateAmountAction;
    public UnityAction<float> updatePercentAction;
    public UnityAction<float> updateCalculateAction;

    public StatusElement()
    {

    }

    public StatusElement(StatusType type, string name, float amount, float percent)
    {
        this.type = type;
        this.name = name;
        this.amount = amount;
        this.percent = percent;
    }
    public StatusElement(StatusElement copyElement)
    {
        this.type = copyElement.type;
        this.name = copyElement.name;
        this.amount = copyElement.amount;
        this.percent = copyElement.percent;
    }

    public virtual float CalculateAmount(float origin)
    {
        return origin + amount;
    }

    public virtual float GetPercentAmount(float origin)
    {
        return origin * percent;
    }

    public virtual float CalculatePercent(float origin)
    {
        return origin + (origin * percent);
    }

    public void SetAmount(float amount)
    {
        this.amount = amount;
        updateAmountAction?.Invoke(this.amount);
    }

    public void AddAmount(float amount) {
        this.amount += amount;
        this.amount = Mathf.Max(0, this.amount);
        updateAmountAction?.Invoke(this.amount);
    }

    public void SubAmount(float amount)
    {
        this.amount -= amount;
        this.amount = Mathf.Max(0, this.amount);
        updateAmountAction?.Invoke(this.amount);
    }

    public void MultiplyAmount(float amount)
    {
        this.amount *= amount;
        this.amount = Mathf.Max(0, this.amount);
        updateAmountAction?.Invoke(this.amount);
    }
    public void DivideAmount(float amount)
    {
        this.amount /= amount;
        this.amount = Mathf.Max(0, this.amount);
        updateAmountAction?.Invoke(this.amount);
    }

    public float GetAmount() { 
        return amount;
    }

    public void SetPercent(float percent)
    {
        this.percent = percent;
        updatePercentAction?.Invoke(this.percent);
    }

    public void AddPercent(float percent)
    {
        this.percent += percent;
        this.percent = Mathf.Max(0, this.percent);
        updatePercentAction?.Invoke(this.percent);
    }
    public void SubPercent(float percent)
    {
        this.percent -= percent;
        this.percent = Mathf.Max(0, this.percent);
        updatePercentAction?.Invoke(this.percent);
    }
    public void MultiplyPercent(float percent)
    {
        this.percent *= percent;
        this.percent = Mathf.Max(0, this.percent);
        updatePercentAction?.Invoke(this.percent);
    }
    public void DividePercent(float percent)
    {
        this.percent /= percent;
        this.percent = Mathf.Max(0, this.percent);
        updatePercentAction?.Invoke(this.percent);
    }

    public float GetPercent()
    {
        return percent;
    }
}