using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameOverPopup : UIBasePopup
{

    private UIGameOverPopupData popupData;

    public override void Init(UIData uiData)
    {
        //��� ������ ����
        popupData = uiData as UIGameOverPopupData;
    }
}
