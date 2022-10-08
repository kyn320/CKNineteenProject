using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class PlayerController : MonoBehaviour, IDamageable
{
    private PlayerInputController inputController;

    private UnitStatus status;

    [SerializeField]
    private PlayerBattleStateType battleStateType;

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

    public UnityEvent<DamageInfo> damageEvent;

    public UnityEvent<float> updateMoveSpeedEvent;

    [SerializeField]
    private bool isDeath = false;

    private void Awake()
    {
        status = GetComponent<UnitStatus>();
        inputController = GetComponent<PlayerInputController>();
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        ChangeState(PlayerStateType.Idle);
    }

    private void Update()
    {
        //TODO :: If None Battle in Time(5 sec), Update Battle State => Normal
        //UpdateBattleState(PlayerBattleStateType.Normal);
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

    public UnitStatus GetStatus()
    {
        return status;
    }

    public Animator GetAnimator()
    {
        return animator;
    }

    public PlayerInputController GetInputController()
    {
        return inputController;
    }

    public void Jump()
    {
        if (!IsGround()
            && currentStateType != PlayerStateType.Idle
            && currentStateType != PlayerStateType.Move)
            return;

        ChangeState(PlayerStateType.Jump);
    }

    public bool OnDamage(DamageInfo damageInfo)
    {
        if (status.isDeath || currentStateType == PlayerStateType.Hit || currentStateType == PlayerStateType.CriticalHit)
            return false;

        var isDeath = status.OnDamage(damageInfo.damage);
        damageEvent?.Invoke(damageInfo);

        if (isDeath)
        {
            ChangeState(PlayerStateType.Death);
        }
        else if (damageInfo.isCritical)
        {
            ChangeState(PlayerStateType.CriticalHit);
        }
        else
        {
            ChangeState(PlayerStateType.Hit);
        }

        return isDeath;
    }

    public void UpdateBattleState(PlayerBattleStateType battleStateType)
    {
        this.battleStateType = battleStateType;
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
            ,out slopeHit
            ,slopeRayDistance
            ,groundMask
            ))
        {
            var collider = slopeHit.collider;
            var angle = Vector3.Angle(Vector3.up, slopeHit.normal);

            return angle < slopeLimit && angle != 0f;
        }

        return false;
    }

    public Vector3 GetSlopeDirection(Vector3 moveDirection)
    {
        Debug.Log(Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized);
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(footPointTransform.position + Vector3.up * slopeRayOffset, Vector3.down * slopeRayDistance, Color.red);
        Gizmos.DrawWireSphere(footPointTransform.position, groundCheckRadius);

        if (slopeHit.collider != null)
            Gizmos.DrawSphere(slopeHit.point, groundCheckRadius);
    }

}
