using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIAmountText : UIBaseText
{
    public UnityEvent updateAmountEvent;

    public virtual void UpdateAmount(int amount)
    {
        text.text = $"{frontAdditionalText}{(string.IsNullOrEmpty(viewFormat) ? amount : string.Format("{0:"+ viewFormat + "}", amount))}{backAdditionalText}";
        updateAmountEvent?.Invoke();
    }

    public virtual void UpdateAmount(float amount)
    {
        text.text = $"{frontAdditionalText}{(string.IsNullOrEmpty(viewFormat) ? amount : string.Format("{0:" + viewFormat + "}", amount))}{backAdditionalText}";
        updateAmountEvent?.Invoke();
    }

}
