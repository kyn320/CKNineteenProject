using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAmountText : UIBaseText
{
    public virtual void UpdateAmount(int amount)
    {
        text.text = $"{frontAdditionalText}{(string.IsNullOrEmpty(viewFormat) ? amount : string.Format("{0:"+ viewFormat + "}", amount))}{backAdditionalText}";
    }

    public virtual void UpdateAmount(float amount)
    {
        text.text = $"{frontAdditionalText}{(string.IsNullOrEmpty(viewFormat) ? amount : string.Format("{0:" + viewFormat + "}", amount))}{backAdditionalText}";
    }

}
