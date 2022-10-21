using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public abstract class MonsterStateBase : MonoBehaviour
{
    protected MonsterController controller;

    [SerializeField]
    protected MonsterStateType stateType;

    [SerializeField]
    protected List<AnimatorTriggerData> enterAnimatorTriggerList;
    [SerializeField]
    protected List<AnimatorTriggerData> exitAnimatorTriggerList;

    [ReadOnly]
    [ShowInInspector]
    protected bool isStay = false;

    public UnityEvent enterEvent;
    public UnityEvent exitEvent;


    protected virtual void Awake()
    {
        controller = GetComponent<MonsterController>();
        controller.AddState(stateType, this);

    }
    public abstract void Enter();

    public abstract void Update();

    public abstract void Exit();

}



