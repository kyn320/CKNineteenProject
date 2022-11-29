using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerStateBase
{
    [SerializeField]
    private float gravityScale = 2f;

    private float moveSpeed = 0f;
    [SerializeField]
    private float airMoveSpeedMultiplyer = 2f;

    private Animator animator;
    private Vector3 inputVector;

    [SerializeField]
    private SFXPrefabData sfxPrefabData;

    [SerializeField]
    private VFXPrefabData vfxPrefabData;
    bool isGroundEffect;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        animator = controller.GetAnimator();
    }

    public override void Enter()
    {
        animator = controller.GetAnimator();

        animator.SetTrigger("Airial");
        animator.SetBool("IsGrounded", false);

        moveSpeed = controller.GetStatus().currentStatus.GetElement(StatusType.MoveSpeed).CalculateTotalAmount();

        controller.GetRigidbody().useGravity = false;
        for (var i = 0; i < enterAnimatorTriggerList.Count; ++i)
        {
            enterAnimatorTriggerList[i].Invoke(controller.GetAnimator());
        }

        isStay = true;

        enterEvent?.Invoke();
    }

    public override void Update()
    {
        return;
    }

    private void FixedUpdate()
    {
        var rigidBody = controller.GetRigidbody();

        if (inputVector.z > 0)
            inputVector.z = 0;

        Vector3 viewVector = transform.forward * inputVector.z; //+ transform.right * (inputVector.z < 0f ? -inputVector.x : inputVector.x);

        var velocity = rigidBody.velocity;
        var isGround = controller.IsGround();
        animator.SetBool("IsGrounded", isGround);
        Debug.Log($"isGroundEffect : {isGroundEffect}");


        if (velocity.y < 0 && isGround)
        {
            if (isGroundEffect)
            {
                isGroundEffect = false;

                GameObject effect = Instantiate(vfxPrefabData.GetVFXPrefab("Jump"), transform);
                effect.transform.parent = null;
            }

            if (inputVector.magnitude > 0)
                controller.ChangeState(PlayerStateType.Move);
            else
                controller.ChangeState(PlayerStateType.Idle);
        }
        else
            isGroundEffect = true;

        if (rigidBody.velocity.z > moveSpeed || rigidBody.velocity.z < -moveSpeed)
        {
            if(rigidBody.velocity.z > 0)
                rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, moveSpeed);
            else
                rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, -moveSpeed);
        }

        if (rigidBody.velocity.x > moveSpeed || rigidBody.velocity.x < -moveSpeed)
        {
            if (rigidBody.velocity.x > 0)
                rigidBody.velocity = new Vector3(moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
            else
                rigidBody.velocity = new Vector3(-moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
        }

        rigidBody.AddForce(Physics.gravity * gravityScale
            + viewVector * airMoveSpeedMultiplyer * moveSpeed, ForceMode.Acceleration);

    }

    public void UpdateMoveInput(Vector3 inputVector)
    {
        if (!isStay)
            return;

        this.inputVector = inputVector;
    }

    public override void Exit()
    {
        isStay = false;

        exitEvent?.Invoke();
    }
}