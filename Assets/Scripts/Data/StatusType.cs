using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum StatusType
{
    None,
    /// <summary>
    /// 최소 이동속도
    /// </summary>
    MinMoveSpeed,
    /// <summary>
    /// 최대 이동 속도
    /// </summary>
    MaxMoveSpeed,
    /// <summary>
    /// 속도 증감량
    /// </summary>
    IncreaseMoveSpeed,
    /// <summary>
    /// 속도 감소량
    /// </summary>
    DecreaseMoveSpeed,
    /// <summary>
    /// 회전 주기
    /// </summary>
    RotationChangeTime,
    /// <summary>
    /// 회전 속도
    /// </summary>
    RotateSpeed,
    /// <summary>
    /// 넉백 파워
    /// </summary>
    KnockBackPower,
    /// <summary>
    /// 넉백 저항
    /// </summary>
    KnockBackDefence,
}
