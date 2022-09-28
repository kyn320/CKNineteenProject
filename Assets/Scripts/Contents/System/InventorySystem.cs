using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class InventorySystem : MonoBehaviour
{
    [SerializeField]
    private ItemDataContainer itemDataContainer;

    [SerializeField]
    private List<ItemData> haveItems = new List<ItemData>();

    public bool ContainsItem(int itemID)
    {
        return haveItems.Find(item => item.ID == itemID) != null;
    }

    public bool ContainsItem(ItemData itemData)
    {
        return haveItems.Contains(itemData);
    }

    public bool AddItem(int itemID)
    {
        haveItems.Add(itemDataContainer.FindItem(itemID));
        return true;
    }

    public bool AddItem(ItemData itemData)
    {
        haveItems.Add(itemData);
        return true;
    }

    public bool SubItem(int itemID)
    {
        haveItems.Add(itemDataContainer.FindItem(itemID));
        return true;
    }

    public bool SubItem(ItemData itemData)
    {
        haveItems.Add(itemData);
        return true;
    }

}
