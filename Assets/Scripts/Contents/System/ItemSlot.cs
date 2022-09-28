using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ItemSlot
{
    [SerializeField]
    private int index;
    public int Index { get { return index; } }

    [SerializeField]
    private ItemData itemData;
    public int ItemID { get { return itemData.ID; } }
    public bool IsUnique { get { return itemData.IsUnique; } }

    [SerializeField]
    private int amount;
    public int Amount { get { return amount; } }

    [SerializeField]
    private bool isEquiped = false;
    public bool IsEquiped { get { return isEquiped; } }

    [SerializeField]
    private int equippedIndex = -1;
    public int EquippedIndex { get { return equippedIndex; } }

    public UnityAction<int> useAction;

    public ItemSlot()
    {

    }

    public ItemSlot(int index, ItemData itemData)
    {
        this.index = index;
        this.itemData = itemData;
        this.amount = 0;
    }

    public ItemSlot(int index, ItemData itemData, int amount)
    {
        this.index = index;
        this.itemData = itemData;
        this.amount = amount;
    }

    public void UpdateIndex(int index)
    {
        this.index = index;
    }

    public ItemData GetItemData()
    {
        return itemData;
    }

    public void AddAmount(int amount)
    {
        this.amount += amount;
    }

    public bool AllowSub(int amount) {
        return this.amount - amount > 0;
    }

    public bool SubAmount(int amount)
    {
        if (this.amount - amount < 0)
        {
            return false;
        }

        this.amount -= amount;
        return true;
    }

    public void Equip(int equippedIndex)
    {
        isEquiped = true;
        this.equippedIndex = equippedIndex;
    }

    public void UnEquip()
    {
        this.isEquiped = false;
        this.equippedIndex = -1;
    }

    public void UpdateEquippedIndex(int index)
    {
        this.equippedIndex = index;
    }

    public void Use()
    {
        useAction?.Invoke(index);
    }

}
