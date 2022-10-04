using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour, IDamageable
{
    [SerializeField]
    UnitStatus status;

    [SerializeField]
    private MonsterType monsterType;
    [SerializeField]
    private MonsterStateType currentStateType;

    [SerializeField]
    private SerializableDictionary<MonsterStateType, MonsterStateBase> statesDic
        = new SerializableDictionary<MonsterStateType, MonsterStateBase>();

    [SerializeField]
    private Transform target;

    [SerializeField]
    private Animator animator;

    private void Start()
    {
        ChangeState(MonsterStateType.MONSTERSTATE_IDLE);
    }

    public void SetTarget(Transform target) { 
        this.target = target;
    }

    public Transform GetTarget() { 
        return target;
    }

    public void AddState(MonsterStateType stateType, MonsterStateBase state)
    {
        statesDic.Add(stateType, state);
    }

    public void ChangeState(MonsterStateType state)
    {
        statesDic[currentStateType].Exit();

        foreach (var stateBehaviour in statesDic.Values)
        {
            stateBehaviour.enabled = false;
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

    public Animator GetAnimator() { 
        return animator;
    }


    public void Death()
    {
        //TODO :: »ç¸Á Ã³¸®
        gameObject.SetActive(false);
    }

    public bool OnDamage(DamageInfo damageInfo, Vector3 hitPoint, Vector3 hitNormal)
    {
        Debug.Log($"{gameObject.name} :: Damage = {damageInfo.damage}");
        status.OnDamage(damageInfo.damage);
        return false;
    }
}
