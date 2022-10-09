using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerIdleState : PlayerStateBase
{
    [SerializeField]
    private float recoverTickTime;

    [ReadOnly]
    [ShowInInspector]
    private float currentRecoverTickTime;

    [SerializeField]
    private float changeNormalBattleTime = 5f;

    [ReadOnly]
    [ShowInInspector]
    private float currentChangeBattleTime;

    public override void Enter()
    {
        for (var i = 0; i < enterAnimatorTriggerList.Count; ++i)
        {
            enterAnimatorTriggerList[i].Invoke(controller.GetAnimator());
        }

        var animator = controller.GetAnimator();
        animator.SetInteger("MoveSpeed", 0);
        animator.SetBool("IsGrounded", true);

        isStay = true;
        controller.updateMoveSpeedEvent?.Invoke(0f);
        currentChangeBattleTime = changeNormalBattleTime;
        currentRecoverTickTime = recoverTickTime;
        enterEvent?.Invoke();
    }

    public void UpdateMoveInput(Vector3 inputVector)
    {
        if (!isStay)
            return;

        if (inputVector.magnitude > 0)
            controller.ChangeState(PlayerStateType.Move);
    }

    public void UpdateForwardView(Vector3 forwardView)
    {
        if (!isStay)
            return;

        forwardView.y = 0;
        transform.forward = forwardView;
    }

    public override void Update()
    {
        switch (controller.GetBattleState())
        {
            case PlayerBattleStateType.Normal:
                currentRecoverTickTime -= Time.deltaTime;
                if (currentRecoverTickTime <= 0f)
                {
                    currentRecoverTickTime = recoverTickTime;
                    controller.GetStatus().AutoRecover();
                }
                break;
            case PlayerBattleStateType.Battle:
                currentChangeBattleTime -= Time.deltaTime;
                if (currentChangeBattleTime <= 0f)
                {
                    controller.UpdateBattleState(PlayerBattleStateType.Normal);
                }
                break;
        }
    }

    private void FixedUpdate()
    {
        controller.GetRigidbody().velocity = Vector3.zero;
    }

    public override void Exit()
    {
        isStay = false;
        controller.updateMoveSpeedEvent?.Invoke(1f);
        exitEvent?.Invoke();
    }
}
