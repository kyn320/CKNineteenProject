using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class UIProjectileSlot : MonoBehaviour
{
    private RectTransform rectTransform;

    [SerializeField]
    private GameObject visibleObject;

    [SerializeField]
    private ItemSlot itemSlot;

    [SerializeField]
    private UIBaseImage itemIcon;

    [SerializeField]
    private UIAmountText amountText;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void UpdateItemSlot(ItemSlot itemSlot)
    {

        if (itemSlot == null)
        {
            visibleObject.SetActive(false);
            return;
        }

        visibleObject.SetActive(itemSlot.IsEquiped);

        if (!itemSlot.IsEquiped)
            return;

        this.itemSlot = itemSlot;

        var itemData = itemSlot.GetItemData();

        if (itemData == null)
        {
            return;
        }

        itemIcon.SetImage(itemData.Icon);
        amountText.UpdateAmount(itemSlot.Amount);
    }

    public void Move(Vector2 movePosition)
    {
        rectTransform.anchoredPosition = movePosition;
    }

    public void Scale(float scaleSize)
    {
        rectTransform.localScale = new Vector3(scaleSize, scaleSize, 1f);
    }

}
