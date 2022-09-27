using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum StatusType
{
    None,
    /// <summary>
    /// 체력
    /// </summary>
    HP,
    /// <summary>
    /// 최대 체력
    /// </summary>
    MaxHP,
    /// <summary>
    /// 체력 회복량
    /// </summary>
    RecoverHP,
    /// <summary>
    /// 최소 공격력
    /// </summary>
    MinAttackPower,
    /// <summary>
    /// 최대 공격력
    /// </summary>
    MaxAttackPower,
    /// <summary>
    /// 방어력
    /// </summary>
    Defence,
    /// <summary>
    /// 공격 속도
    /// </summary>
    AttackSpeed,
    /// <summary>
    /// 공격 사거리
    /// </summary>
    AttackDistance,
    /// <summary>
    /// 공격 사거리
    /// </summary>
    ThrowSpeed,
    /// <summary>
    /// 치명타 확률
    /// </summary>
    CriticalPercent,
    /// <summary>
    /// 치명타 공격력
    /// </summary>
    CriticalAttackPower,
    /// <summary>
    /// 이동 속도
    /// </summary>
    MoveSpeed,
    /// <summary>
    /// 점프력
    /// </summary>
    JumpPower,
    /// <summary>
    /// 시야 거리
    /// </summary>
    SightDistance,
    /// <summary>
    /// 시야 각
    /// </summary>
    SightDegree,
    /// <summary>
    /// 시야 높이
    /// </summary>
    SightHeight,
    /// <summary>
    /// 공격 선 딜레이
    /// </summary>
    BeforeAttackTime,
    /// <summary>
    /// 공격 후 딜레이
    /// </summary>
    AfterAttackTime,

}
