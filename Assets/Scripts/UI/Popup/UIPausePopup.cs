using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPausePopup : UIBasePopup
{
    public override void Init(UIData uiData)
    {
        TPSMouseSetting.Instance.OpenUICursor();
    }

    public override void EndOpen()
    {
        base.EndOpen();
        Time.timeScale = 0f;
    }

    public void OnEnterTitle()
    {
        Time.timeScale = 1f;
        SceneLoader.Instance.SwitchScene("TitleScene");
    }

    public override void BeginClose()
    {
        Time.timeScale = 1f;
        TPSMouseSetting.Instance.CloseUICursor();
        base.BeginClose();
    }
}
