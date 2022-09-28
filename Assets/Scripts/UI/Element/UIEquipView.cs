using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEquipView : MonoBehaviour
{
    [SerializeField]
    private BezierCurveUGUI curveUGUI;

    [SerializeField]
    private List<UIEquipSlot> equipSlots;

    public void Start()
    {
        RefreshSlots();
        EquipmentSystem.Instance.updateEquippedItems.AddListener(RefreshSlots);
    }

    public void RefreshSlots(List<ItemSlot> equipItems)
    {
        RefreshSlots();
    }

    public void RefreshSlots()
    {
        var itemSlots = EquipmentSystem.Instance.GetEquipSlots();

        if (itemSlots.Count < 1)
            return;

        for (var i = 0; i < equipSlots.Count; ++i)
        {
            equipSlots[i].UpdateItemSlot(itemSlots[i]);
        }
    }

}
