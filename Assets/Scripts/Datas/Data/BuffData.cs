using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Linq;

[CreateAssetMenu(fileName = "BuffData", menuName = "Buff/BuffData", order = 0)]
public class BuffData : ScriptableObject
{
    [SerializeField]
    private BuffType type;
    public BuffType Type { get { return type; } }

    [SerializeField]
    private List<CrowdType> crowdTypes;
    public List<CrowdType> CrowdTypes { get { return crowdTypes; } }

    [SerializeField]
    private string buffName;
    public string BuffName { get { return buffName; } }

    [SerializeField]
    private string description;
    public string Description { get { return description; } }

    [SerializeField]
    private Sprite buffIcon;
    public Sprite BuffIcon { get { return buffIcon; } }

    [SerializeField]
    private float lifeTime;
    public float LifeTime { get { return lifeTime; } }

    [SerializeField]
    private bool isOverlap = false;
    public bool IsOverlap { get { return isOverlap; } }

    [SerializeField]
    private SerializableDictionary<StatusType, StatusElement> statusDic = new SerializableDictionary<StatusType, StatusElement>();
    public SerializableDictionary<StatusType, StatusElement> StausDic { get { return statusDic; } }

    [SerializeField]
    private GameObject buffBehaviourObject;
    public GameObject BuffBehaviourObject { get { return buffBehaviourObject; } }

    [Button("AutoGenerate")]
    public void AutoGenerate()
    {
        statusDic.Clear();

        IEnumerable<StatusType> StatusTypeList =
                Enum.GetValues(typeof(StatusType)).Cast<StatusType>();

        foreach (StatusType statusType in StatusTypeList)
        {
            if (statusType == StatusType.None)
                continue;

            statusDic.Add(statusType, new StatusElement() { name = statusType.ToString(), type = statusType });
        }
    }
}
