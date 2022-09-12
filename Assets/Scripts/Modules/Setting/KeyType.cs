using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum KeyType 
{
    None,
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
    /// 캐릭터 변경
    /// </summary>
    ChangeCharacter,

    /// <summary>
    /// 스킬 슬롯 1
    /// </summary>
    Skill1,
    /// <summary>
    /// 스킬 슬롯 2
    /// </summary>
    Skill2,

    /// <summary>
    /// 상태
    /// </summary>
    Status,
    /// <summary>
    /// 메뉴(일시정지)
    /// </summary>
    Menu,
}
