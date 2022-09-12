using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIYesOrNoPopupData : UIPopupData
{
    public UnityAction yesAction;
    public UnityAction noAction;

    public UIYesOrNoPopupData() {
        viewName = "YesOrNo";
    }

}
