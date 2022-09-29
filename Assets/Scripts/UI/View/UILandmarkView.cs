using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILandmarkView : UIBaseView
{
    [SerializeField]
    UIBaseGauge hpGauge;

    private UILandmarkViewData hudData;

    public override void Init(UIData uiData)
    {
        hudData = uiData as UILandmarkViewData;


    }


}
