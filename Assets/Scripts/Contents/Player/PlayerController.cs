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
    private CharacterController characterController;

    public UnityEvent<DamageInfo> damageEvent;

    [ReadOnly]
    [ShowInInspector]
    private Vector3 moveVector;

    public UnityEvent<float> updateMoveSpeedEvent;

    [SerializeField]
    private bool isDeath = false;

    private void Awake()
    {
        status = GetComponent<UnitStatus>();
        inputController = GetComponent<PlayerInputController>();
        characterController = GetComponent<CharacterController>();
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
        if (!characterController.isGrounded
            && currentStateType != PlayerStateType.Idle
            && currentStateType != PlayerStateType.Move)
            return;

        ChangeState(PlayerStateType.Jump);
    }

    public bool OnDamage(DamageInfo damageInfo)
    {
        if(status.isDeath || currentStateType == PlayerStateType.Hit || currentStateType == PlayerStateType.CriticalHit)
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

    public Vector3 GetMoveVector()
    {
        return moveVector;
    }

    public void SetMoveVector(Vector3 moveVector)
    {
        this.moveVector = moveVector;
    }

    public bool GetIsGround()
    {
        return characterController.isGrounded;
    }
}
