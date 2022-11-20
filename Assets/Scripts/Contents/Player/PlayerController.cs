using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class PlayerController : MonoBehaviour, IDamageable, IHitPauseable
{
    private PlayerInputController inputController;

    private PlayerStatus status;

    [SerializeField]
    private PlayerBattleStateType battleStateType = PlayerBattleStateType.Normal;

    [SerializeField]
    private PlayerStateType currentStateType;
    [SerializeField]
    public List<PlayerStateType> allowParllexStateTypeList;

    [SerializeField]
    private SerializableDictionary<PlayerStateType, PlayerStateBase> statesDic
        = new SerializableDictionary<PlayerStateType, PlayerStateBase>();

    [SerializeField]
    private Animator animator;
    private new Rigidbody rigidbody;

    [SerializeField]
    private float slopeLimit;
    [SerializeField]
    private Transform footPointTransform;
    [SerializeField]
    private float groundCheckRadius;
    [SerializeField]
    private LayerMask groundMask;
    [SerializeField]
    private float slopeRayOffset;
    [SerializeField]
    private float slopeRayDistance;
    private RaycastHit slopeHit;

    [SerializeField]
    private Transform stairCheckTransform;
    [SerializeField]
    private float stairCheckRadius;
    [SerializeField]
    private float stairRayOffset;
    [SerializeField]
    private float stairRayDistance;
    private RaycastHit stairHit;

    public UnityEvent<PlayerBattleStateType> updateBattleStateEvent;

    public UnityEvent<DamageInfo> damageEvent;
    private Coroutine hitPauseCoroutine;

    public UnityEvent<float> updateMoveSpeedEvent;

    [SerializeField]
    private bool isDeath = false;
    [SerializeField]
    private bool isAttack = false;

    private void Awake()
    {
        status = GetComponent<PlayerStatus>();
        inputController = GetComponent<PlayerInputController>();
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        updateBattleStateEvent?.Invoke(battleStateType);
        ChangeState(PlayerStateType.Idle);
    }

    public void AddState(PlayerStateType stateType, PlayerStateBase state)
    {
        statesDic.Add(stateType, state);
    }
    public void ChangeState(PlayerStateType state)
    {
        statesDic[currentStateType].Exit();

        foreach (var key in statesDic.Keys)
        {

            if (allowParllexStateTypeList.Contains(key))
                continue;

            statesDic[key].enabled = false;
        }

        currentStateType = state;
        Debug.Log($"Player :: ChangeState >> { currentStateType }");

        statesDic[currentStateType].enabled = true;
        statesDic[currentStateType].Enter();
    }

    public PlayerStateType GetState()
    {
        return currentStateType;
    }

    public PlayerStatus GetStatus()
    {
        return status;
    }

    public Animator GetAnimator()
    {
        return animator;
    }

    public void SetStateEnbaled(PlayerStateType type, bool isTrigger)
    {
        statesDic[type].enabled = isTrigger;
    }

    public PlayerInputController GetInputController()
    {
        return inputController;
    }

    public void Jump()
    {
        if(isAttack)
            return;

        if (!IsGround()
            && currentStateType != PlayerStateType.Idle
            && currentStateType != PlayerStateType.Move)
            return;

        ChangeState(PlayerStateType.Jump);
    }
    public void SetIsAttack(bool isAttack)
    {
        this.isAttack = isAttack;
    }

    public virtual DamageInfo OnDamage(DamageInfo damageInfo)
    {
        if (status.isDeath || currentStateType == PlayerStateType.Hit || currentStateType == PlayerStateType.CriticalHit)
        {
            damageInfo.isHit = false;
            damageInfo.isKill = false;

            return damageInfo;
        }

        UpdateBattleState(PlayerBattleStateType.Battle);

        var resultDamageInfo = status.OnDamage(damageInfo);
        damageEvent?.Invoke(resultDamageInfo);
        if (isDeath)
        {
            ChangeState(PlayerStateType.Death);
        }
        else if (damageInfo.isCritical && damageInfo.isKnockBack)
        {
            ChangeState(PlayerStateType.CriticalHit);
        }
        else if (damageInfo.isKnockBack)
        {
            ChangeState(PlayerStateType.Hit);
        }

        return resultDamageInfo;
    }

    public PlayerBattleStateType GetBattleState()
    {
        return battleStateType;
    }

    public void UpdateBattleState(PlayerBattleStateType battleStateType)
    {
        this.battleStateType = battleStateType;
        updateBattleStateEvent?.Invoke(battleStateType);
    }

    public Rigidbody GetRigidbody()
    {
        return rigidbody;
    }

    public bool IsGround()
    {
        var collisions = Physics.OverlapSphere(footPointTransform.position, groundCheckRadius, groundMask);
        return collisions.Length > 0;
    }

    public bool CheckSlope()
    {
        if (Physics.Raycast(footPointTransform.position + Vector3.up * slopeRayOffset
            , Vector3.down
            , out slopeHit
            , slopeRayDistance
            , groundMask
            ))
        {
            var collider = slopeHit.collider;
            var angle = Vector3.Angle(Vector3.up, slopeHit.normal);

            return angle < slopeLimit && angle != 0f;
        }

        return false;
    }

    public RaycastHit GetGroundedRaycastHit()
    {
        Physics.Raycast(footPointTransform.position + Vector3.up * slopeRayOffset
                , Vector3.down
                , out slopeHit
                , slopeRayDistance
                , groundMask
                );

        return slopeHit;
    }

    public RaycastHit GetStairCastHit()
    {
        Physics.SphereCast(stairCheckTransform.position + Vector3.up * stairRayOffset
                , stairCheckRadius
                , Vector3.down
                , out stairHit
                , stairRayDistance
                , groundMask
                );

        return stairHit;
    }

    public Vector3 GetSlopeDirection(Vector3 moveDirection)
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    public virtual void HitPause(float playWaitTime, float lifeTime)
    {
        if (hitPauseCoroutine != null)
        {
            StopCoroutine(hitPauseCoroutine);
        }

        hitPauseCoroutine = StartCoroutine(CoHitPause(playWaitTime, lifeTime));
    }

    public virtual IEnumerator CoHitPause(float playWaitTime, float lifeTime)
    {
        yield return new WaitForSeconds(playWaitTime);
        animator.speed = 0;
        yield return new WaitForSeconds(lifeTime);
        animator.speed = 1f;
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(footPointTransform.position + Vector3.up * slopeRayOffset, Vector3.down * slopeRayDistance, Color.red);
        Debug.DrawRay(stairCheckTransform.position + Vector3.up * stairRayOffset, Vector3.down * stairRayDistance, Color.blue);

        Gizmos.DrawWireSphere(footPointTransform.position, groundCheckRadius);

        if (slopeHit.collider != null)
            Gizmos.DrawSphere(slopeHit.point, groundCheckRadius);

        if(stairHit.collider != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(stairHit.point, groundCheckRadius);
        }
    }

}
