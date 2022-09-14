using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum KeyType 
{
    None,
    /// <summary>
    /// 앞으로 이동
    /// </summary>
    MoveFoward,
    /// <summary>
    /// 뒤로 이동
    /// </summary>
    MoveBack,
    /// <summary>
    /// 왼쪽 이동
    /// </summary>
    MoveLeft,
    /// <summary>
    /// 오른쪽 이동
    /// </summary>
    MoveRight,

    /// <summary>
    /// 점프
    /// </summary>
    Jump,
    /// <summary>
    /// 대시
    /// </summary>
    Dash,
    /// <summary>
    /// 공격 
    /// </summary>
    Attack,
    /// <summary>
    /// 상호작용
    /// </summary>
    Interaction,
   
    /// <summary>
    /// 메뉴(일시정지)
    /// </summary>
    Menu,
}
