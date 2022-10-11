using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class BuffController : MonoBehaviour
{
    [SerializeField]
    private UnitStatus status;

    public List<BuffBehaviour> buffBehaviourList;

    private void Awake()
    {
        status = GetComponent<UnitStatus>();
    }

    public void AddBuff(BuffData buffData)
    {
        var buffObject = Instantiate(buffData.BuffBehaviourObject, transform);

        var buffBehaviour = buffObject.GetComponent<BuffBehaviour>();

        buffBehaviourList.Add(buffBehaviour);

        buffBehaviour.SetBuffData(buffData);
        buffBehaviour.SetBuffController(this);
        buffBehaviour.StartBuff();
    }

    public void SubBuff(BuffData buffData)
    {
        var buffBehaviour = buffBehaviourList.Find(item => item.EqualBuffData(buffData));
        buffBehaviour.EndBuff();
    }

    public void SubBuff(string buffName)
    {

    }
    public void SubBuff(CrowdType crowdType)
    {

    }

    public void RemoveBuffList(BuffBehaviour buffBehaviour)
    {
        if (buffBehaviour == null)
            return;

        buffBehaviourList.Remove(buffBehaviour);
        Destroy(buffBehaviour.gameObject);
    }

    public UnitStatus GetStatus()
    {
        return status;
    }

}
