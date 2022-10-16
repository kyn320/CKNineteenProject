using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBuffView : UIBaseView
{
    [SerializeField]
    private GameObject uiBuffSlotPrefab;

    [SerializeField]
    private RectTransform buffArea;
    [SerializeField]
    private RectTransform debuffArea;

    public override void Init(UIData uiData)
    {

    }

    public void CreateBuffSlot(BuffBehaviour buffBehaviour)
    {
        RectTransform buffGroup = null;

        switch (buffBehaviour.GetBuffData().Type)
        {
            case BuffType.Buff:
                buffGroup = buffArea;
                break;
            case BuffType.Debuff:
                buffGroup = debuffArea;
                break;
        }

        var buffSlotObject = Instantiate(uiBuffSlotPrefab, buffGroup);
        var buffSlot = buffSlotObject.GetComponent<UIBuffSlot>();

        buffSlot.SetBuff(buffBehaviour);
    }

}
