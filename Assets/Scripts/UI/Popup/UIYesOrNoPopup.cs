using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIYesOrNoPopup : UIBasePopup
{
    private UIYesOrNoPopupData yesOrNoPopupData;

    [SerializeField]
    protected Text descriptionText;

    public UnityEvent yesEvent;
    public UnityEvent noEvent;

    public override void Init(UIData uiData)
    {
        yesOrNoPopupData = uiData as UIYesOrNoPopupData;

        viewName = yesOrNoPopupData.viewName;
        descriptionText.text = yesOrNoPopupData.descrition;
        yesEvent.AddListener(yesOrNoPopupData.yesAction);
        noEvent.AddListener(yesOrNoPopupData.noAction);
    }

    public void OnYes() {
        yesEvent?.Invoke();
        Close();
    }

    public void OnNo()
    {
        noEvent?.Invoke();
        Close();
    }

}
