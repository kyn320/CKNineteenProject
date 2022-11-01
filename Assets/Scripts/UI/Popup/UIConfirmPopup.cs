using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIConfirmPopup : UIBasePopup
{
    private UIConfirmPopupData confirmPopupData;

    [SerializeField]
    protected Text descriptionText;
    
    public UnityEvent confirmEvent;

    public override void Init(UIData uiData)
    {
        confirmPopupData = uiData as UIConfirmPopupData;
        viewName = confirmPopupData.viewName;
        descriptionText.text = confirmPopupData.descrition;
    }

    public void OnConfirm() {
        confirmEvent?.Invoke();
        Close();
    }

}
