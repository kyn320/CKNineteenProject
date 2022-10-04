using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterStateBase : MonoBehaviour
{
    protected MonsterController controller;

    [SerializeField]
    protected MonsterStateType stateType;

    [SerializeField]
    protected List<AnimatorTriggerData> enterAnimatorTriggerList;
    [SerializeField]
    protected List<AnimatorTriggerData> exitAnimatorTriggerList;

    protected virtual void Awake()
    {
        controller = GetComponent<MonsterController>();
        controller.AddState(stateType, this);

    }
    public abstract void Enter();

    public abstract void Update();

    public abstract void Exit();

}



