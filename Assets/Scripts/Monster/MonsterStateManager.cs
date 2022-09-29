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
    private GameObject target;

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

        InitializeStates();
        PlayAction(monster.GetState());

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
        Debug.Log("Change Monster State : " + state.ToString());
    }

    public void SetTarget(GameObject obj)
    {
        target = obj;
    }

    public GameObject GetTarget()
    {
        return target;
    }
    public Vector3 GetTargetPosition()
    {
        return target.transform.localPosition;
    }
}
