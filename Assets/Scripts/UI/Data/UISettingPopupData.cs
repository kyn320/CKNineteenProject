using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UISettingPopupData : UIPopupData
{
    public UnityAction endCloseEvent; 

    public UISettingPopupData()
    {
        viewName = "Setting";
    }

}
