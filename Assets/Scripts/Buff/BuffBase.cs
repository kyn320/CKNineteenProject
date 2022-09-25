using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffBase : MonoBehaviour
{
    BuffTestPlayer player;
    BuffType buffType = BuffType.NONE;
    float buffCount = .0f;
    float buffEffectTime = .0f;

    float currentTime = .0f;
    bool isBuffEnded = false;
    

    public BuffBase(BuffType buff, float count, float effectTime)
    {
        buffType = buff;
        buffCount = count;
        buffEffectTime = effectTime;

        player = GetComponent<BuffTestPlayer>();
    }

    private void calcBuffTimer(float time)
    {
        currentTime += Time.deltaTime;
        // ���� ���� ��

        if(currentTime > time)
        {
            currentTime = .0f;
            Debug.Log(buffType + " ���� ����");
            isBuffEnded = true;
        }
    }

    public bool GetActiveBuff()
    {
        return isBuffEnded;
    }

    public float GetBuffTime()
    {
        return currentTime;
    }
}
