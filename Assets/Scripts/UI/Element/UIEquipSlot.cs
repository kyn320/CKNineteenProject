using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIEquipSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private GameObject visibleObject;

    [SerializeField]
    private ItemSlot itemSlot;

    [SerializeField]
    private UIBaseImage itemIcon;

    [SerializeField]
    private UIAmountText amountText;

    public void UpdateItemSlot(ItemSlot itemSlot)
    {
        visibleObject.SetActive(itemSlot != null);

        if (itemSlot == null)
            return;

        this.itemSlot = itemSlot;

        var itemData = itemSlot.GetItemData();

        if (itemData == null) {
            return;
        }

        itemIcon.SetImage(itemData.Icon);
        amountText.UpdateAmount(itemSlot.Amount);
    }

    public void UnEquip()
    {
        EquipmentSystem.Instance.UnEquipItem(itemSlot.EquippedIndex);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left:
                break;
            case PointerEventData.InputButton.Right:
                UnEquip();
                break;
            case PointerEventData.InputButton.Middle:
                break;
        }
    }

}
