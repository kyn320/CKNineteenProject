using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIProfilePanel : MonoBehaviour
{
    [SerializeField]
    private UICompareAmountText hpAmountText;
    [SerializeField]
    private UIBaseGauge hpGauge;

    public void UpdateHP(float current, float max)
    {
        hpAmountText.UpdateAmount(current, max);
        hpGauge.UpdateGauge(current, max);
    }
}
