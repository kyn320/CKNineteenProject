using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuffBase
{
    public StatusType type;
    public float effectTime;
    public float count;
    public float currentCount;

    public BuffBase(StatusType type, float effectTime, float count, float currentCount = 0)
    {
        this.type = type;
        this.effectTime = effectTime;
        this.count = count;
        this.currentCount = currentCount;
    }
}
[System.Serializable]
public class BuffTimer
{
    public float timerCount;
}

[System.Serializable]
public class Buffer
{
    public StatusInfoData datas;
    public BuffTimer timer;
    public List<BuffBase> buff;
}

public class BuffBuffer : Singleton<BuffBuffer>
{
    public StatusInfoData playerStatus;

    public Buffer buffBuffer;

    public Buffer deBuffBuffer;

    private void Update()
    {
        UpdateBuffTimer();
        UpdateDeBuffTimer();
    }


    public void CreateBuffer(StatusType type, float time, float count, float currentCount)
    {
        if(CheckNewBuff(type, 1))
        {
            buffBuffer.buff.Add(new BuffBase(type, buffBuffer.timer.timerCount + time, count, currentCount));
            buffBuffer.datas.StausDic[type].AddAmount(count);
            IncreaseStatus(type, buffBuffer.datas.StausDic[type].GetAmount());
        } else
        {
           for(var i=0;i<buffBuffer.buff.Count;i++)
            {
                if(buffBuffer.buff[i].type == type)
                {
                    buffBuffer.buff[i].effectTime += time;
                    buffBuffer.buff[i].count = count;
                }
            }
        }
    }

    public void CreateDeBuffer(StatusType type, float time, float count, float currentCount)
    {
        if (CheckNewBuff(type, 2))
        {
            deBuffBuffer.buff.Add(new BuffBase(type, deBuffBuffer.timer.timerCount + time, count, currentCount));
            deBuffBuffer.datas.StausDic[type].AddAmount(count);
            DecreaseStatus(type, deBuffBuffer.datas.StausDic[type].GetAmount());
        }
        else
        {
            for (var i = 0; i < deBuffBuffer.buff.Count; i++)
            {
                if (deBuffBuffer.buff[i].type == type)
                {
                    deBuffBuffer.buff[i].effectTime += time;
                    deBuffBuffer.buff[i].count = -count;
                }
            }
        }
    }

    bool CheckNewBuff(StatusType type, int buffType = 0) 
    {
        if(buffType == 0)
        {
            for(var i=0;i<buffBuffer.buff.Count;i++)
            {
                if (buffBuffer.buff[i].type == type)
                    return false;
                else
                    return true;
            }
        }
        else if(buffType == 1)
        {
            for(var i=0;i<deBuffBuffer.buff.Count;i++)
            {
                if (deBuffBuffer.buff[i].type == type)
                    return true;
                else
                    return false;
            }
        }
        return true;
    }

    void IncreaseStatus(StatusType type, float count)
    {
        playerStatus.StausDic[type].AddAmount(count);
    }

    void DecreaseStatus(StatusType type, float count)
    {
        playerStatus.StausDic[type].SubAmount(count);
    }

    void UpdateBuffTimer()
    {
        buffBuffer.timer.timerCount += Time.deltaTime;

        for (var i = 0; i < buffBuffer.buff.Count; i++)
        {
            if(buffBuffer.timer.timerCount >= buffBuffer.buff[i].effectTime)
            {
                DecreaseStatus(buffBuffer.buff[i].type, buffBuffer.buff[i].count);
                buffBuffer.buff.Remove(buffBuffer.buff[i]);
            }
        }

        if(buffBuffer.buff.Count <= 0 )
        {
            buffBuffer.timer.timerCount = 0;
        }
    }

    void UpdateDeBuffTimer()
    {
        deBuffBuffer.timer.timerCount += Time.deltaTime;

        for (var i = 0; i < deBuffBuffer.buff.Count; i++)
        {
            if (deBuffBuffer.timer.timerCount >= deBuffBuffer.buff[i].effectTime)
            {
                IncreaseStatus(deBuffBuffer.buff[i].type, deBuffBuffer.buff[i].count);
                deBuffBuffer.buff.Remove(deBuffBuffer.buff[i]);
            }
        }

        if (deBuffBuffer.buff.Count <= 0)
        {
            deBuffBuffer.timer.timerCount = 0;
        }
    }
    public StatusInfoData ActiveBuffer()
    {
        return buffBuffer.datas;
    }

    public StatusInfoData ActiveDeBuffer()
    {
        return deBuffBuffer.datas;
    }


}
