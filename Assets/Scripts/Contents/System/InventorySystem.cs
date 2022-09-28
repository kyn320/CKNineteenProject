using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class InventorySystem : Singleton<InventorySystem>
{
    [SerializeField]
    private ItemDataContainer itemDataContainer;

    [SerializeField]
    private List<ItemSlot> haveItems = new List<ItemSlot>();

    private ItemSlot GetSlot(int ItemID)
    {
        return haveItems.Find(haveItems => haveItems.ItemID == ItemID);

    }

    private ItemSlot GetSlot(ItemData ItemData)
    {
        return GetSlot(ItemData.ID);

    }

    public List<ItemSlot> GetItemSlots()
    {
        return haveItems;
    }

    public bool ContainsItem(int itemID)
    {
        return GetSlot(itemID) != null;
    }

    public bool ContainsItem(ItemData itemData)
    {
        return ContainsItem(itemData.ID);
    }

    [Button("아이템 추가")]
    public bool AddItem(int itemID)
    {
        var itemData = itemDataContainer.FindItem(itemID);

        AddItem(itemData, 1);
        return true;
    }

    public bool AddItem(int itemID, int itemAmount)
    {
        var itemData = itemDataContainer.FindItem(itemID);

        AddItem(itemData, itemAmount);
        return true;
    }

    public bool AddItem(ItemData itemData)
    {
        AddItem(itemData, 1);
        return true;
    }

    public bool AddItem(ItemData itemData, int itemAmount)
    {
        var slot = GetSlot(itemData);

        if (!itemData.IsUnique && slot != null)
        {
            slot.AddAmount(itemAmount);
        }
        else
        {
            haveItems.Add(new ItemSlot(haveItems.Count, itemData, itemAmount));
        }

        return true;
    }

    [Button("아이템 제거")]
    public bool SubItem(int itemID, int amount)
    {
        var itemSlot = GetSlot(itemID);

        if (itemSlot == null)
            return false;

        var isSuccess = itemSlot.SubAmount(amount);

        if (isSuccess && itemSlot.Amount <= 0)
        {
            haveItems.Remove(itemSlot);
            RefreshSlotIndexs();
        }

        return isSuccess;
    }

    public bool SubItem(ItemData itemData, int amount)
    {
        return SubItem(itemData.ID, amount);
    }

    [Button("아이템 사용")]
    public ItemData Use(int slotIndex)
    {
        return Use(slotIndex, 1);
    }

    public ItemData Use(int slotIndex, int amount)
    {
        if (slotIndex < 0 || slotIndex > haveItems.Count - 1)
            return null;

        var itemSlot = haveItems[slotIndex];

        if (itemSlot == null)
        {
            Debug.LogError($"[InventorySystem] : 아이템 슬롯 (Index = {slotIndex})이 Null 입니다.");
            return null;
        }

        if ((!itemSlot.IsUnique && itemSlot.SubAmount(1)) || itemSlot.IsUnique)
        {
            return itemSlot.GetItemData();
        }

        return null;
    }

    public void RefreshSlotIndexs()
    {
        for (var i = 0; i < haveItems.Count; ++i)
        {
            haveItems[i].UpdateIndex(i);
        }
    }

}
