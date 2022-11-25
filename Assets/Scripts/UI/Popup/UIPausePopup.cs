using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPausePopup : UIBasePopup
{
    public override void Init(UIData uiData)
    {
        TPSMouseSetting.Instance.OpenUICursor();
    }
    
    public void OnEnterTitle() { 
        SceneLoader.Instance.SwitchScene("TitleScene");
    }

    public override void BeginClose()
    {
        TPSMouseSetting.Instance.CloseUICursor();
        base.BeginClose();
    }
}
