using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class UICompareAmountText : UIAmountText
{
    public UIColorData colorData;

    public string normalColorKey = "Normal";

    public bool usePercentDisplay = true;
    public bool useDynamicColor = true;
    public bool useRequireAmountDisplay = false;

    public string lackColorKey = "Lack";
    public string equalColorKey = "Equal";
    public string overColorKey = "Over";

    [ShowInInspector]
    [ReadOnly]
    private float requireAmount = 0f;

    public void UpdateAmount(int current, int requireAmount)
    {
        var color = colorData.colorDic[normalColorKey];

        if (useDynamicColor)
        {
            if (current == requireAmount)
            {
                color = colorData.colorDic[equalColorKey];
            }
            else if (current < requireAmount)
            {
                color = colorData.colorDic[lackColorKey];
            }
            else
            {
                color = colorData.colorDic[overColorKey];
            }
        }

        var displayText = "";

        if (usePercentDisplay)
        {
            var percent = (current / (float)requireAmount) * 100f;
            displayText = string.IsNullOrEmpty(viewFormat) ? percent.ToString() : string.Format("{0:" + viewFormat + "}", percent);
        }
        else if (useRequireAmountDisplay)
        {
            displayText = string.IsNullOrEmpty(viewFormat) ? string.Format("{0}/{1}", current, requireAmount)
                : string.Format("{0:" + viewFormat + "}/{1:" + viewFormat + "}", current);
        }
        else
        {
            displayText = string.IsNullOrEmpty(viewFormat) ? current.ToString() : string.Format("{0:" + viewFormat + "}", current);
        }

        text.text = $"{frontAdditionalText}<color=#{ColorUtility.ToHtmlStringRGBA(color)}>{displayText}</color>{backAdditionalText}";
    }

    public void UpdateAmount(float current, float requireAmount)
    {
        var color = colorData.colorDic[normalColorKey];

        if (useDynamicColor)
        {
            if (current == requireAmount)
            {
                color = colorData.colorDic[equalColorKey];
            }
            else if (current < requireAmount)
            {
                color = colorData.colorDic[lackColorKey];
            }
            else
            {
                color = colorData.colorDic[overColorKey];
            }
        }

        var displayText = "";

        if (usePercentDisplay)
        {
            var percent = (current / (float)requireAmount) * 100f;
            displayText = string.IsNullOrEmpty(viewFormat) ? percent.ToString() : string.Format("{0:" + viewFormat + "}", percent);
        }
        else if (useRequireAmountDisplay)
        {
            displayText = string.IsNullOrEmpty(viewFormat) ? string.Format("{0}/{1}", current, requireAmount)
                : string.Format("{0:" + viewFormat + "}/{1:" + viewFormat + "}", current);
        }
        else
        {
            displayText = string.IsNullOrEmpty(viewFormat) ? current.ToString() : string.Format("{0:" + viewFormat + "}", current);
        }

        text.text = $"{frontAdditionalText}<color=#{ColorUtility.ToHtmlStringRGBA(color)}>{displayText}</color>{backAdditionalText}";
    }

}
