using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class UIProjectileSlot : MonoBehaviour
{
    private RectTransform rectTransform;

    private List<ItemSlot> equipItemSlotList = new List<ItemSlot>();
    private int viewIndex = 0;

    [SerializeField]
    private GameObject visibleObject;

    [SerializeField]
    private ItemSlot itemSlot;

    [SerializeField]
    private UIBaseImage itemIcon;

    //[SerializeField]
    //private UIAmountText amountText;

    public UnityEvent updateSlotEvent;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        EquipmentSystem.Instance.updateAttackOrderList.AddListener(UpdateAttackOrderList);
    }

    public void UpdateViewIndex(int viewIndex)
    {
        this.viewIndex = viewIndex;

        visibleObject.SetActive(equipItemSlotList.Count != 0);
        if (equipItemSlotList.Count == 0)
            return;

        UpdateItemSlot(equipItemSlotList[viewIndex]);
    }

    public void UpdateAttackOrderList(List<ItemSlot> itemSlotList)
    {
        equipItemSlotList = itemSlotList;
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
        //amountText.UpdateAmount(itemSlot.Amount);
        updateSlotEvent?.Invoke();
    }

}
