using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIAlertPopup : UIBasePopup
{
    [SerializeField]
    protected UIBaseText alertMessageText;

    private UIAlertPopupData alertData;

    [SerializeField]
    protected float viewTime = 3f;
    public override void Init(UIData uiData)
    {
        alertData = uiData as UIAlertPopupData;

        alertMessageText.SetText(alertData.alertMessage);
    }

    public override void EndOpen()
    {
        base.EndOpen();
        StartCoroutine(CoWaitForViewTime());
    }

    IEnumerator CoWaitForViewTime()
    {
        yield return new WaitForSeconds(viewTime);
        Close();
    }

}
