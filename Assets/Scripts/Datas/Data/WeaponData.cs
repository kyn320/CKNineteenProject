using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Linq;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Item/WeaponData", order = 0)]
public class WeaponData : ItemData
{
    [SerializeField]
    private WeaponType weaponType;
    public WeaponType WeaponType { get { return weaponType; } }

}
