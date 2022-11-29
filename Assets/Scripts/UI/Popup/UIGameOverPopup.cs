using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameOverPopup : UIBasePopup
{
    private UIGameOverPopupData popupData;
    public float viewTime = 3f;

    public override void Init(UIData uiData)
    {
        //��� ������ ����
        popupData = uiData as UIGameOverPopupData;
    }

    public override void EndOpen()
    {
        base.EndOpen();
        Invoke("AutoEnterTitle", viewTime);
    }

    public void AutoEnterTitle() {
        SceneLoader.Instance.SwitchScene("TitleScene");
    }


}
