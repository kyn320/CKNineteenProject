using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

public class BuffController : MonoBehaviour
{
    [SerializeField]
    private UnitStatus status;

    public float timer = .0f;

    public List<BuffBase> buffList = null;
    public List<BuffBase> deBuffList = null;

    public UnityEvent startBuff;
    public UnityEvent endBuff;

    public UnityEvent startDeBuff;
    public UnityEvent endDeBuff;

    private int buffCount = 0;
    private void Awake()
    {
        status = GetComponent<UnitStatus>();

    }

    private void Start()
    {
        InitializeBuff();
    }

    private void Update()
    {
        if (buffList.Count > 0 || deBuffList.Count > 0)
        {
            timer += Time.deltaTime;

            foreach(var buff in buffList)
            {
                if(timer >= buff.effectTime)
                {
                    RemoveBuff(buff.statusType);
                    break;
                }
            }
            foreach (var buff in deBuffList)
            {
                if (timer >= buff.effectTime)
                {
                    RemoveDeBuff(buff.statusType);
                    break;
                }
            }
        } else if(buffList.Count <= 0 && deBuffList.Count <= 0)
            timer = .0f;
    }

    public void CreateBuff(StatusType type, float time, float count)
    {
        BuffBase buff = ScriptableObject.CreateInstance<BuffBase>();

        buff.statusType = type;
        buff.effectTime = time;
        buff.abilityCount = count;

        AssetDatabase.CreateAsset(buff, "Assets/ScriptableObjects/Buff/" + gameObject.name + buffCount);

        AddBuff(buff);
    }

    public void CreateDeBuff(StatusType type, float time, float count)
    {
        BuffBase buff = ScriptableObject.CreateInstance<BuffBase>();

        buff.statusType = type;
        buff.effectTime = time;
        buff.abilityCount = count;

        AssetDatabase.CreateAsset(buff, "Assets/ScriptableObjects/Buff/" + gameObject.name + buffCount);

        AddDeBuff(buff);
    }

    private void AddBuff(BuffBase buffBase)
    {
        foreach (var buff in buffList)
        {
            if (buffBase.statusType == buff.statusType)
            {
                buff.effectTime = buffBase.effectTime + timer;
                buff.abilityCount += buffBase.abilityCount;

                return;
            }
        }

        buffBase.effectTime += timer;
        status.currentStatus.GetElement(buffBase.statusType).AddAmount(buffBase.abilityCount);

        startBuff?.Invoke();
        buffList.Add(buffBase);
    }
    private void AddDeBuff(BuffBase buffBase)
    {
        foreach (var buff in deBuffList)
        {
            if (buffBase.statusType == buff.statusType)
            {
                buff.effectTime = buffBase.effectTime + timer;
                buff.abilityCount += buffBase.abilityCount;
            }
        }

        buffBase.effectTime += timer;
        status.currentStatus.GetElement(buffBase.statusType).DivideAmount(buffBase.abilityCount);
        startDeBuff?.Invoke();
        deBuffList.Add(buffBase);
    }

    public void RemoveBuff(StatusType type)
    {
        BuffBase deleteBuff = GetBuff(type);
        status.currentStatus.GetElement(deleteBuff.statusType).DivideAmount(deleteBuff.abilityCount);
        endBuff?.Invoke();
        buffList.Remove(deleteBuff);
    }

    public void RemoveDeBuff(StatusType type)
    {
        BuffBase deleteBuff = GetDeBuff(type);
        status.currentStatus.GetElement(deleteBuff.statusType).AddAmount(deleteBuff.abilityCount);
        endDeBuff.Invoke();
        deBuffList.Remove(deleteBuff);
    }

    private BuffBase GetBuff(StatusType type)
    {
        foreach(var buff in buffList)
        {
            if(buff.statusType == type)
                return buff;
        }
        return null;
    }
    private BuffBase GetDeBuff(StatusType type)
    {
        foreach (var buff in deBuffList)
        {
            if (buff.statusType == type)
                return buff;
        }
        return null;
    }

    private void InitializeBuff()
    {
        foreach(var buff in buffList)
        {
            status.currentStatus.GetElement(buff.statusType).AddAmount(buff.abilityCount);
        }
        foreach(var deBuff in deBuffList)
        {
            status.currentStatus.GetElement(deBuff.statusType).DivideAmount(deBuff.abilityCount);
        }
    }
}
