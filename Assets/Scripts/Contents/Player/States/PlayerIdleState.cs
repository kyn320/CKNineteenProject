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


    [SerializeField]
    private float delayToMoveStop = 1f;
    private float currentDelayToMoveStop = 0f;
    private bool isDelayMoveStop = false;

    private bool isAttack = false;

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
        isDelayMoveStop = controller.GetPrevState() == PlayerStateType.Move;
        currentDelayToMoveStop = delayToMoveStop;
        enterEvent?.Invoke();
    }

    public void UpdateMoveInput(Vector3 inputVector)
    {
        if (!isStay || isAttack)
            return;

        if (inputVector.magnitude > 0)
            controller.ChangeState(PlayerStateType.Move);
    }

    public void UpdateForwardView(Vector3 forwardView)
    {
        if (!isStay || isAttack)
            return;

        forwardView.y = 0;
        transform.forward = forwardView;
    }

    public void SetIsAttack(bool isAttack)
    {
        this.isAttack = isAttack;
    }

    public override void Update()
    {
        if (isDelayMoveStop)
        {
            currentDelayToMoveStop -= Time.deltaTime;

            if (currentDelayToMoveStop <= 0f)
            {
                isDelayMoveStop = false;
                currentDelayToMoveStop = 0f;
            }
        }

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
        if (!controller.IsGround())
        {
            controller.ChangeState(PlayerStateType.Air);
        }
        else
        {
            if (isAttack)
                return;

            if (!isDelayMoveStop)
                controller.GetRigidbody().velocity = Vector3.zero;
        }
    }

    public override void Exit()
    {
        isStay = false;
        controller.updateMoveSpeedEvent?.Invoke(1f);
        exitEvent?.Invoke();
    }
}
