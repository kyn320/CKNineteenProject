using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIConfirmPopupData : UIPopupData
{
    public UnityAction confirmAction;

    public UIConfirmPopupData() {
        viewName = "Confirm";
    }

}
