using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Linq;

[CreateAssetMenu(fileName = "CrowdActionData", menuName = "Crowd/CrowdActionData", order = 0)]

public class CrowdActionData : ScriptableObject
{
    //상태이상 타입 
    [SerializeField]
    private CrowdType type;

    public CrowdType Type { get { return type; } }


    [SerializeField]
    private CrowdTimerType timerType;
    public CrowdTimerType TimerType { get { return timerType; } }

    //상태이상 지속 시간
    [SerializeField]
    private float time;
    public float Time { get { return time; } }

    [SerializeField]
    private GameObject crowdObject;
    
    public GameObject CrowdObject { get { return crowdObject; } }   
}
