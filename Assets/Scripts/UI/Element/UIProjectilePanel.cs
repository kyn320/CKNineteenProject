using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class UIProjectilePanel : UIListView<UIProjectileSlot>
{
    private RectTransform rectTransform;

    [SerializeField]
    private float spacingX = 20f;

    [SerializeField]
    private Vector2 currentSlotSize;

    [SerializeField]
    private float currentSlotScale;

    [SerializeField]
    private Vector2 nextSlotSize;

    [SerializeField]
    private float nextSlotScale;

    [ReadOnly]
    [ShowInInspector]
    private int currentIndex = 0;

    [ReadOnly]
    [ShowInInspector]
    private int equipCount = 0;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        EquipmentSystem.Instance.updateAttackOrderList.AddListener(UpdateAttackOrder);
    }

    private void Update()
    {
        //TEST ¿ë
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Next();
        }
    }

    public void UpdateAttackOrder(List<ItemSlot> itemSlots)
    {
        RemoveAll();

        equipCount = itemSlots.Count;

        for (var i = 0; i < equipCount; ++i)
        {
            var content = AddContent();
            content.UpdateItemSlot(itemSlots[i]);
        }

        UpdateSlots();
    }

    public void Next()
    {
        currentIndex = (int)Mathf.Repeat(currentIndex + 1, contentList.Count);
        UpdateSlots();

    }

    public void UpdateSlots() {
        if (equipCount < 1)
            return;

        if (equipCount < 4)
        {

        }
        else
        {
            for (var i = 0; i < contentList.Count; ++i)
            {
                var viewIndex = (int)Mathf.Repeat(currentIndex + i, contentList.Count);

                Debug.Log(currentIndex);
                Debug.Log((currentIndex + i) + " / " + viewIndex);

                if (i == 0)
                {
                    contentList[viewIndex].Scale(currentSlotScale);
                    contentList[viewIndex].Move(Vector2.zero);
                }
                else
                {
                    var position = Vector2.right;
                    position *= currentSlotSize.x;
                    position.x += i < 2? (i * nextSlotSize.x * 0.5f) : (nextSlotSize.x * (i -1) + i * nextSlotSize.x * 0.5f);

                    contentList[viewIndex].Scale(nextSlotScale);
                    contentList[viewIndex].Move(position);
                }
            }
        }
    }

}
