using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ItemGiver : MonoBehaviour
{
    [SerializeField]
    private ItemData itemData;

    public void SetItemData(ItemData itemData) { 
        this.itemData = itemData;
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
