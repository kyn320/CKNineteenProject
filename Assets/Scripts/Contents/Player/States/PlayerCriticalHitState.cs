using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerCriticalHitState : PlayerStateBase
{
    [SerializeField]
    private Vector3 knockBackVector;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void Enter()
    {
        if(isStay)
            return;

        for (var i = 0; i < enterAnimatorTriggerList.Count; ++i)
        {
            enterAnimatorTriggerList[i].Invoke(controller.GetAnimator());
        }

        isStay = true;

        var knockBackDirection = transform.forward * knockBackVector.z + transform.up * knockBackVector.y;
        controller.GetRigidbody().velocity = Vector3.zero;
        controller.GetRigidbody().AddForce(knockBackDirection, ForceMode.Impulse);
        enterEvent?.Invoke();
    }

    public override void Update()
    {
        return;
    }

    public override void Exit()
    {
        isStay = false;
        controller.GetRigidbody().velocity = Vector3.zero;

        for (var i = 0; i < exitAnimatorTriggerList.Count; ++i)
        {
            exitAnimatorTriggerList[i].Invoke(controller.GetAnimator());
        }
        exitEvent?.Invoke();
    }

    public void EndHit()
    {
        if (isStay)
            controller.ChangeState(PlayerStateType.Idle);
    }
}
