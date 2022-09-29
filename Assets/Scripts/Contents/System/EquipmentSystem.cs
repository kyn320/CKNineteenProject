using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class EquipmentSystem : Singleton<EquipmentSystem>
{
    [SerializeField]
    private List<ItemSlot> equippedItems = new List<ItemSlot>();

    [SerializeField]
    private List<ItemSlot> attackOrderList = new List<ItemSlot>();


    public UnityEvent<List<ItemSlot>> updateEquippedItems;
    public UnityEvent<List<ItemSlot>> updateAttackOrderList;


    [Button("������ ���")]
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

    public List<ItemSlot> GetEquipSlots()
    {
        return equippedItems;
    }


    public void UpdateAttackOrder()
    {
        attackOrderList.Clear();

        for (var i = 0; i < equippedItems.Count; ++i)
        {
            var equipSlot = equippedItems[i];

            if (equipSlot == null || !equippedItems[i].IsEquiped)
            {
                continue;
            }

            attackOrderList.Add(equipSlot);
        }

        updateAttackOrderList?.Invoke(attackOrderList);
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
                UpdateAttackOrder();
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
        UpdateAttackOrder();

        return true;
    }



}
