using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class EquipmentSystem : Singleton<EquipmentSystem>
{
    [SerializeField]
    private List<ItemSlot> equippedItems = new List<ItemSlot>();

    public UnityEvent<List<ItemSlot>> updateEquippedItems;

    [Button("아이템 사용")]
    public bool Use(int slotIndex)
    {
        var equipSlot = GetEquipSlot(slotIndex);

        if (equipSlot == null)
            return false;


        var itemData = InventorySystem.Instance.Use(equipSlot.Index);

        return itemData != null;
    }

    public ItemSlot GetEquipSlot(int slotIndex)
    {
        return equippedItems[slotIndex];
    }

    public List<ItemSlot> GetEquipSlots() {
        return equippedItems;
    }

    public bool EquipItem(ItemSlot itemSlot)
    {
        for (var i = 0; i < equippedItems.Count; ++i)
        {
            if (equippedItems[i] == null || !equippedItems[i].IsEquiped)
            {
                itemSlot.Equip(i);
                equippedItems[i] = itemSlot;
                updateEquippedItems?.Invoke(equippedItems);
                return true;
            }
        }

        return false;
    }

    public bool UnEquipItem(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex > equippedItems.Count - 1 || equippedItems[slotIndex] == null)
            return false;

        equippedItems[slotIndex].UnEquip();
        equippedItems[slotIndex] = null;

        updateEquippedItems?.Invoke(equippedItems);

        return true;
    }



}
