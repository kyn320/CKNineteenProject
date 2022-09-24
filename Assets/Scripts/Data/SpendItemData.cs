using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Linq;

[CreateAssetMenu(fileName = "SpendItemData", menuName = "Item/SpendItemData", order = 0)]
public class SpendItemData : ItemData
{

    [SerializeField]
    private SpendItemType spendItemType;
    public SpendItemType SpendItemType { get { return spendItemType; } }

}
