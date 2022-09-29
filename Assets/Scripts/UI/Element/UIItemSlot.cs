using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIItemSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private ItemSlot itemSlot;

    [SerializeField]
    private UIItemSlotImageData slotImageData;

    [SerializeField]
    private UIBaseImage itemTypeIcon;

    [SerializeField]
    private UIBaseImage equippedTagIcon;

    [SerializeField]
    private UIBaseImage itemIcon;

    [SerializeField]
    private UIBaseText nameText;

    [SerializeField]
    private UIAmountText amountText;

    public void UpdateItemSlot(ItemSlot itemSlot)
    {
        this.itemSlot = itemSlot;

        itemSlot.equipAction += UpdateEquip;

        var itemData = itemSlot.GetItemData();

        itemTypeIcon.SetImage(slotImageData.ItemTypeSpriteDic[itemData.Type]);

        equippedTagIcon.gameObject.SetActive(itemSlot.IsEquiped);

        itemIcon.SetImage(itemData.Icon);
        nameText.SetText(itemData.Name);
        amountText.UpdateAmount(itemSlot.Amount);
    }

    public void Equip()
    {
        if (itemSlot.IsEquiped)
            return;

        EquipmentSystem.Instance.EquipItem(itemSlot);
    }

    public void UpdateEquip(bool isEquip)
    {
        equippedTagIcon.gameObject.SetActive(isEquip);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left:
                break;
            case PointerEventData.InputButton.Right:
                Equip();
                break;
            case PointerEventData.InputButton.Middle:
                break;
        }
    }
}