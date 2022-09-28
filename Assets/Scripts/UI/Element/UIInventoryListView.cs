using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryListView : UIListView<UIItemSlot>
{

    public void UpdateSlots()
    {
        var itemSlots = InventorySystem.Instance.GetItemSlots();

        RemoveAll();

        for (var i = 0; i < itemSlots.Count; ++i)
        {
            var itemSlot = itemSlots[i];

            var slotContent = AddContent();

            slotContent.UpdateItemSlot(itemSlot);
        }


    }


}
