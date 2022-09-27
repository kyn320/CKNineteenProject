using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum StatusType
{
    None,
    /// <summary>
    /// ü��
    /// </summary>
    HP,
    /// <summary>
    /// �ִ� ü��
    /// </summary>
    MaxHP,
    /// <summary>
    /// ü�� ȸ����
    /// </summary>
    RecoverHP,
    /// <summary>
    /// �ּ� ���ݷ�
    /// </summary>
    MinAttackPower,
    /// <summary>
    /// �ִ� ���ݷ�
    /// </summary>
    MaxAttackPower,
    /// <summary>
    /// ����
    /// </summary>
    Defence,
    /// <summary>
    /// ���� �ӵ�
    /// </summary>
    AttackSpeed,
    /// <summary>
    /// ���� ��Ÿ�
    /// </summary>
    AttackDistance,
    /// <summary>
    /// ���� ��Ÿ�
    /// </summary>
    ThrowSpeed,
    /// <summary>
    /// ġ��Ÿ Ȯ��
    /// </summary>
    CriticalPercent,
    /// <summary>
    /// ġ��Ÿ ���ݷ�
    /// </summary>
    CriticalAttackPower,
    /// <summary>
    /// �̵� �ӵ�
    /// </summary>
    MoveSpeed,
    /// <summary>
    /// ������
    /// </summary>
    JumpPower,
    /// <summary>
    /// �þ� �Ÿ�
    /// </summary>
    SightDistance,
    /// <summary>
    /// �þ� ��
    /// </summary>
    SightDegree,
    /// <summary>
    /// �þ� ����
    /// </summary>
    SightHeight,
    /// <summary>
    /// ���� �� ������
    /// </summary>
    BeforeAttackTime,
    /// <summary>
    /// ���� �� ������
    /// </summary>
    AfterAttackTime,

}
