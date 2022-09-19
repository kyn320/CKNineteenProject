using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStateManager : MonoBehaviour
{
    [SerializeField]
    private MonsterBase monster;

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
                Debug.Break();
            }
        }

        rig = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();

        InitializeStates();
    }

    private void Start()
    {
        PlayAction(monster.GetState());
    }

    private void InitializeStates()
    {
        states.Add(MonsterState.MONSTERSTATE_IDLE, GetComponent<MonsterIdleState>());
        states.Add(MonsterState.MONSTERSTATE_PATROLL, GetComponent<MonsterPatrollState>());
        states.Add(MonsterState.MONSTERSTATE_ATTACK, GetComponent<MonsterAttackState>());
        states.Add(MonsterState.MONSTERSTATE_TRACKING, GetComponent<MonsterTrackingState>());
        states.Add(MonsterState.MONSTERSTATE_SUFFER, GetComponent<MonsterSufferState>());
        states.Add(MonsterState.MONSTERSTATE_DEATH, GetComponent<MonsterDeathState>());
    }

    public void PlayAction(MonsterState state)
    {
        foreach(StateBase temp in states.Values)
        {
            Debug.Log(temp);

            temp.enabled = false;
        }

        if(monster.GetState().ToString() != state.ToString())
            monster.SetState(state);

        states[monster.GetState()].enabled = true;
        states[monster.GetState()].Action();
        Debug.Log("Change Monster State : " + state.ToString());
    }

    public void SetTarget(GameObject obj)
    {
        target = obj;
    }

    public Vector3 GetTargetPosition()
    {
        return target.transform.localPosition;
    }

}
