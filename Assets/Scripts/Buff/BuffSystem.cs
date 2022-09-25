using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BuffTestPlayer))]
public class BuffSystem : BuffBuffer
{
    private BuffTestPlayer buffPlayer;

    private void Awake()
    {
        buffPlayer = GetComponent<BuffTestPlayer>();

        if(buffPlayer == null)
        {
            Debug.LogError(buffPlayer.GetType() + "is Not Found");
        }
    }

    public void CreateBuff(BuffType type, float buffCount, float effectTime)
    {
        AddBuffer(new BuffBase(type, buffCount, effectTime));
    }

    // CreateBuff ( buffType , buffCount, effectTime )
    // ������ �������ְ�, ���ۿ� �����Ѵ�.
    // ����� ���ۿ����� ������ �����ϸ�, ������ effectTime�� ����Ǿ��� ���, �����Ѵ�.

    // CreateDeBuff ( buffType, buffCount, effectTime )

    // DeleteBuff(buffType)
    // buffType�� �ش��ϴ� buff�� ���ۿ��� �����Ѵ�.
    // DeleteDeBuff(buffType)
}
