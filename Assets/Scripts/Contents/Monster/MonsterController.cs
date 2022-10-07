using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class MonsterController : MonoBehaviour, IDamageable
{
    private UnitStatus status;

    [SerializeField]
    private MonsterType monsterType;
    [SerializeField]
    private MonsterStateType currentStateType;
    [SerializeField]
    public List<MonsterStateType> allowParllexStateTypeList;

    [SerializeField]
    private SerializableDictionary<MonsterStateType, MonsterStateBase> statesDic
        = new SerializableDictionary<MonsterStateType, MonsterStateBase>();

    [SerializeField]
    private Transform target;

    [SerializeField]
    private Animator animator;

    public UnityEvent<DamageInfo> damageEvent;

    private void Awake()
    {
        status = GetComponent<UnitStatus>();
    }

    private void Start()
    {
        ChangeState(MonsterStateType.MONSTERSTATE_IDLE);
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public Transform GetTarget()
    {
        return target;
    }

    public void AddState(MonsterStateType stateType, MonsterStateBase state)
    {
        statesDic.Add(stateType, state);
    }

    public void ChangeState(MonsterStateType state)
    {
        statesDic[currentStateType].Exit();

        foreach (var key in statesDic.Keys)
        {

            if (allowParllexStateTypeList.Contains(key))
                continue;

            statesDic[key].enabled = false;
        }

        currentStateType = state;
        Debug.Log($"Monster :: ChangeState >> { currentStateType }");

        statesDic[currentStateType].enabled = true;
        statesDic[currentStateType].Enter();
    }

    public MonsterStateType GetState()
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


    public void Death()
    {
        //TODO :: »ç¸Á Ã³¸®
        gameObject.SetActive(false);
    }

    public virtual bool OnDamage(DamageInfo damageInfo)
    {
        Debug.Log($"{gameObject.name} :: Damage = {damageInfo.damage}");
        var isDeath = status.OnDamage(damageInfo.damage);
        if (isDeath)
        {
            ChangeState(MonsterStateType.MONSTERSTATE_DEATH);
        }
        else
        {
            if(damageInfo.isCritical)
            {
                ChangeState(MonsterStateType.MONSTERSTATE_CRITICALHIT);
            }
            else {
                ChangeState(MonsterStateType.MONSTERSTATE_HIT);
            }
        }
        damageEvent?.Invoke(damageInfo);
        return isDeath;
    }
}
