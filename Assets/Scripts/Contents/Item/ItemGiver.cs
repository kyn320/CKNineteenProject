using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ItemGiver : MonoBehaviour
{
    [SerializeField]
    private ItemData itemData;

    [SerializeField]
    private UIBaseImage itemIcon;

    public void SetItemData(ItemData itemData)
    {
        this.itemData = itemData;
        itemIcon.SetImage(itemData.Icon);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            InventorySystem.Instance.AddItem(itemData);
            gameObject.SetActive(false);
        }
    }

}
