using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public abstract class PlayerStateBase : MonoBehaviour
{
    protected PlayerController controller;

    [SerializeField]
    protected PlayerStateType stateType;

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
        controller = GetComponent<PlayerController>();
        controller.AddState(stateType, this);
    }

    public abstract void Enter();

    public abstract void Update();

    public abstract void Exit();

    public void UsedKnockback(bool isTrigger) { }
}
