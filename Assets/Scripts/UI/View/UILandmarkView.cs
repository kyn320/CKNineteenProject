using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILandmarkView : UIBaseView
{
    [SerializeField]
    UIBaseGauge hpGauge;
    [SerializeField]
    UICompareAmountText compareAmountText;

    private UILandmarkViewData hudData;

    public override void Init(UIData uiData)
    {
        hudData = uiData as UILandmarkViewData;
    }

    public void UpdateShieldAmount(float current, float max) {
        hpGauge.UpdateGauge(current,max);
        compareAmountText.UpdateAmount(current,max);
    }



}
