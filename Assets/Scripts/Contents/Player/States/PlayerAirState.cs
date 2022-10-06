using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerStateBase
{
    [SerializeField]
    private float gravity = 9.81f;

    private Animator animator;
    private CharacterController characterController;

    protected override void Awake()
    {
        base.Awake();
        characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        animator = controller.GetAnimator();
    }

    public override void Enter()
    {
        for (var i = 0; i < enterAnimatorTriggerList.Count; ++i)
        {
            enterAnimatorTriggerList[i].Invoke(controller.GetAnimator());
        }

        isStay = true;

        enterEvent?.Invoke();
    }

    public override void Update()
    {
        var moveVector = controller.GetMoveVector();

        animator.SetBool("IsGrounded", characterController.isGrounded);
        moveVector.y -= gravity * Time.deltaTime;

        if (moveVector.y <= 0 && characterController.isGrounded)
        {
            controller.ChangeState(PlayerStateType.Idle);
        }

        controller.SetMoveVector(moveVector);
        characterController.Move(moveVector * Time.deltaTime);

    }

    public override void Exit()
    {
        isStay = false;

        exitEvent?.Invoke();
    }
}