using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameOverPopup : UIBasePopup
{

    private UIGameOverPopupData popupData;

    public override void Init(UIData uiData)
    {
        //통계 데이터 추출
        popupData = uiData as UIGameOverPopupData;
    }
}
