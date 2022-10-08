using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerHitState : PlayerStateBase
{
    [SerializeField]
    private Vector3 knockBackVector;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void Enter()
    {
        for (var i = 0; i < enterAnimatorTriggerList.Count; ++i)
        {
            enterAnimatorTriggerList[i].Invoke(controller.GetAnimator());
        }

        isStay = true;
        var knockBackDirection = transform.forward * knockBackVector.z + transform.up * knockBackVector.y;

        controller.GetRigidbody().AddForce(knockBackDirection, ForceMode.VelocityChange);
        enterEvent?.Invoke();
    }

    public override void Update()
    {
        return;
    }

    public override void Exit()
    {
        isStay = false;
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
