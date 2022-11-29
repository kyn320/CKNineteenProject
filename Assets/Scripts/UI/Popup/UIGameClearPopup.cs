using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameClearPopup : UIBasePopup
{
    public float viewTime = 3f;

    public override void Init(UIData uiData)
    {

    }
    public override void EndOpen()
    {
        base.EndOpen();
        Invoke("AutoEnterTitle", viewTime);
    }

    public void AutoEnterTitle()
    {
        SceneLoader.Instance.SwitchScene("CreditScene");
    }


}
