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
    // 버프를 생성해주고, 버퍼에 저장한다.
    // 저장된 버퍼에서는 버프를 관리하며, 버프의 effectTime이 종료되었을 경우, 삭제한다.

    // CreateDeBuff ( buffType, buffCount, effectTime )

    // DeleteBuff(buffType)
    // buffType에 해당하는 buff를 버퍼에서 제거한다.
    // DeleteDeBuff(buffType)
}
