using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryPopup : UIBasePopup
{

    [SerializeField]
    private UIInventoryListView inventoryListView;

    public override void Init(UIData uiData)
    {

    }

    private void Start()
    {
        inventoryListView.UpdateSlots();
    }




}
