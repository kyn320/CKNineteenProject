using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStateManager : MonoBehaviour
{
    [SerializeField]
    private MonsterBase monster;

    private Rigidbody rig;
    private Collider coll;

    public Dictionary<MonsterState, StateBase> states;

    private void Awake()
    {
        if (monster == null)
        {
            monster = GetComponent<MonsterBase>();

            if (monster == null)
            {
                Debug.LogError(gameObject.name + "is Not Found : GameObject");
                Debug.Break();
            }
        }

        rig = monster.GetComponent<Rigidbody>();
        coll = monster.GetComponent<Collider>();

        InitializeStates();
    }

    private void InitializeStates()
    {
        states.Add(MonsterState.MONSTERSTATE_IDLE, GetComponent<MonsterIdleState>());
        states.Add(MonsterState.MONSTERSTATE_PATROLL, GetComponent<MonsterPatrollState>());
        states.Add(MonsterState.MONSTERSTATE_ATTACK, GetComponent<MonsterAttackState>());
        states.Add(MonsterState.MONSTERSTATE_SUFFER, GetComponent<MonsterSufferState>());
        states.Add(MonsterState.MONSTERSTATE_DEATH, GetComponent<MonsterDeathState>());
    }

    private void PlayAction(MonsterState state)
    {
        foreach(var temp in states.Values)
        {
            temp.enabled = false;
        }

        monster.SetState(state);

        states[monster.GetState()].enabled = true;
        states[monster.GetState()].Action();
    }


}
