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
        bezierCurveUGUI.SetRootCanvas(UIController.Instance.rootCanvas);
    }

    private void Start()
    {
        inventoryListView.UpdateSlots();
    }




}
