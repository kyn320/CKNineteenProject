using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum MonsterState
{
    MONSTERSTATE_IDLE = 0,
    MONSTERSTATE_PATROLL,
    MONSTERSTATE_TRACKING,
    MONSTERSTATE_ATTACK,
    MONSTERSTATE_SUFFER,
    MONSTERSTATE_DEATH
};

public class MonsterBase : MonoBehaviour
{
    [Space(10)]
    [SerializeField]
    private int monsterCode;

    [Space(10)]
    [SerializeField]
    private int hp = 0;

    [Space(10)]
    [SerializeField]
    private float moveSpeed = .0f;
    [SerializeField]
    private float attackSpeed = .0f;
    [SerializeField]
    private float searchRadius = .0f;

    [Space(10)]
    [SerializeField]
    private bool isLife = false;

    [Space(10)]
    [SerializeField]
    private MonsterState states = 0;

    [Space(10)]
    public GameObject dropItem = null;

    public void SetMoveSpeed(float value)
    {
        moveSpeed = value;
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    public void SetAttackSpeed(float value)
    {
        attackSpeed = value;
    }
    public float GetAttackSpeed()
    {
        return attackSpeed;
    }

    public void SetLife(bool isTrigger)
    {
        isLife = isTrigger;
    }

    public bool GetLife()
    {
        return isLife;
    }

    public void SetState(MonsterState state)
    {
        states = state;
    }

    public MonsterState GetState()
    {
        return states;
    }

    public float GetSearchRadius()
    {
        return searchRadius;
    }

    public void SetHP(int num)
    {
        hp = num;
    }

    public int GetHP()
    {
        return hp;
    }

}