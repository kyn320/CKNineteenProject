using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public enum MagicType 
{
    None,
    /// <summary>
    /// 버프
    /// </summary>
    Buff,
    /// <summary>
    /// 디버프
    /// </summary>
    DeBuff,
    /// <summary>
    /// 투사체
    /// </summary>
    Projectile,

}
