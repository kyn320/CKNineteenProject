using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterStateManager : MonoBehaviour
{
    [SerializeField]
    public MonsterBase monster;

    public Rigidbody rig;
    public Collider coll;

    [SerializeField]
    private Dictionary<MonsterState, StateBase> states = new Dictionary<MonsterState, StateBase>();

    public Animator anim;

    [SerializeField]
    public NavMeshAgent agent;

    private void Awake()
    {
        if (monster == null)
        {
            monster = GetComponent<MonsterBase>();

            if (monster == null)
            {
                Debug.LogError(gameObject.name + "is Not Found");
            }
        }

        rig = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
        anim = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();

        InitializeStates();
        PlayAction(monster.GetState());
        agent.speed = monster.monsterStatus.StausDic[StatusType.MoveSpeed].GetAmount();
    }


    private void InitializeStates()
    {
        states.Add(MonsterState.MONSTERSTATE_IDLE, GetComponent<MonsterIdleState>());
        states.Add(MonsterState.MONSTERSTATE_PATROLL, GetComponent<MonsterPatrollState>());
        states.Add(MonsterState.MONSTERSTATE_TRACKING, GetComponent<MonsterTrackingState>());
        states.Add(MonsterState.MONSTERSTATE_ATTACK, GetComponent<MonsterAttackState>());
        states.Add(MonsterState.MONSTERSTATE_SUFFER, GetComponent<MonsterSufferState>());
        states.Add(MonsterState.MONSTERSTATE_DEATH, GetComponent<MonsterDeathState>());
    }

    public void PlayAction(MonsterState state)
    {
        foreach(StateBase temp in states.Values)
        {
            temp.enabled = false;
        }

        if (monster.GetState().ToString() != state.ToString())
            monster.SetState(state);

        states[monster.GetState()].enabled = true;
        states[monster.GetState()].Action();
        anim.SetInteger("CurrentAnim", (int)monster.GetState());
    }

    public void SetTarget(GameObject obj)
    {
        monster.target = obj;
    }

    public GameObject GetTarget()
    {
        return monster.target;
    }
    public Vector3 GetTargetPosition()
    {
        return monster.target.transform.localPosition;
    }
}
