using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UIPopupData : UIData
{
    public string title;
    public string descrition;

    public override string prefabPath => base.prefabPath + "Popup";

    public UIPopupData() {
        uiType = UIType.Popup;
    }

}
