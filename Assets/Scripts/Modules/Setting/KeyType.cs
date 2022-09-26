using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum KeyType 
{
    None,
    /// <summary>
    /// ������ �̵�
    /// </summary>
    MoveFoward,
    /// <summary>
    /// �ڷ� �̵�
    /// </summary>
    MoveBack,
    /// <summary>
    /// ���� �̵�
    /// </summary>
    MoveLeft,
    /// <summary>
    /// ������ �̵�
    /// </summary>
    MoveRight,

    /// <summary>
    /// ����
    /// </summary>
    Jump,
    /// <summary>
    /// ���
    /// </summary>
    Dash,
    /// <summary>
    /// ���� 
    /// </summary>
    Attack,
    /// <summary>
    /// ��ȣ�ۿ�
    /// </summary>
    Interaction,
   
    /// <summary>
    /// �޴�(�Ͻ�����)
    /// </summary>
    Menu,
}
