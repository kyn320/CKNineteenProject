using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryPopup : UIBasePopup
{

    [SerializeField]
    private UIInventoryListView inventoryListView;

    [SerializeField]
    private BezierCurveUGUI bezierCurveUGUI;

    public override void Init(UIData uiData)
    {
        TPSMouseSetting.Instance.OpenUICursor();
        bezierCurveUGUI.SetRootCanvas(UIController.Instance.rootCanvas);
    }

    private void Start()
    {
        inventoryListView.UpdateSlots();
    }

    public override void Close()
    {
        base.Close();
        TPSMouseSetting.Instance.CloseUICursor();
    }



}
