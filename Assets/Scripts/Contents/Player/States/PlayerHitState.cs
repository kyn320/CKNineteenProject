using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerHitState : PlayerStateBase
{
    [SerializeField]
    private float gravity = 1f;
    [SerializeField]
    private float decreseMoveSpeed = 1f;

    [SerializeField]
    private Vector3 knockBackVector;
    [ReadOnly]
    [ShowInInspector]
    private Vector3 moveVector;
    [ReadOnly]
    [ShowInInspector]
    private Vector3 velocity;

    private CharacterController characterController;

    protected override void Awake()
    {
        base.Awake();
        characterController = GetComponent<CharacterController>();
    }

    public override void Enter()
    {
        for (var i = 0; i < enterAnimatorTriggerList.Count; ++i)
        {
            enterAnimatorTriggerList[i].Invoke(controller.GetAnimator());
        }

        isStay = true;

        velocity  = moveVector = transform.forward * knockBackVector.z + transform.up * knockBackVector.y;
        enterEvent?.Invoke();
    }

    public override void Update()
    {
        velocity.y -= gravity * Time.deltaTime;

        velocity.x += transform.forward.x * decreseMoveSpeed * Time.deltaTime;
        velocity.z += transform.forward.z * decreseMoveSpeed * Time.deltaTime;
        velocity.x = moveVector.x > 0 ? Mathf.Max(0,velocity.x) : Mathf.Min(0, velocity.x);
        velocity.z = moveVector.z > 0 ? Mathf.Max(0, velocity.z) : Mathf.Min(0, velocity.z);

        characterController.Move(velocity * Time.deltaTime);
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
