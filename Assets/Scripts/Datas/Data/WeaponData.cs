using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Item/WeaponData", order = 0)]
public class WeaponData : ItemData
{
    [SerializeField]
    private WeaponType weaponType;
    public WeaponType WeaponType { get { return weaponType; } }

    [SerializeField]
    private HandType handType;
    public HandType HandType { get { return handType; } }

    [SerializeField]
    private WeaponAttackType attackType;
    public WeaponAttackType AttackType { get { return attackType; } }


    [SerializeField]
    private ProjectileMoveType projectileMoveType;
    public ProjectileMoveType ProjectileMoveType { get { return projectileMoveType; } }

    [SerializeField]
    private bool isMoveable = false;
    public bool IsMoveable { get { return isMoveable; } }

    [SerializeField]
    private int attackAnimationType;
    public int AttackAnimationType { get { return attackAnimationType; } }

    [SerializeField]
    private int comboCount;
    public int ComboCount { get { return comboCount; } }

    [SerializeField]
    private List<VFXPrefabData> attackVFXDataList;
    public List<VFXPrefabData> AttackVFXDataList { get { return attackVFXDataList; } }

    [SerializeField]
    private List<AttackHitBoxData> hitBoxDataList;
    public List<AttackHitBoxData> HitBoxDataList { get { return hitBoxDataList; } }

    [SerializeField]
    private List<PivotOffsetData> pivotOffsetList;
    public List<PivotOffsetData> PivotOffsetDataList { get { return pivotOffsetList; } }

    [SerializeField]
    private List<GameObject> subWeaponList;
    public List<GameObject> SubWeaponList { get { return subWeaponList; } }
}
