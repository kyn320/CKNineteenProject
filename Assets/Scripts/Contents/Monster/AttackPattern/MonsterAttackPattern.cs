using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public abstract class MonsterAttackPattern : MonoBehaviour
{
    protected MonsterController controller;

    protected Animator animator;

    [SerializeField]
    protected Transform target;

    [SerializeField]
    protected List<AnimatorTriggerData> startAttackTriggerDataList;
    [SerializeField]
    protected List<AnimatorTriggerData> endAttackTriggerDataList;

    [ReadOnly]
    [SerializeField]
    protected bool isCoolDown = false;
    public bool IsCoolDown { get { return isCoolDown; } }
    [SerializeField]
    protected bool isAttacked = false;

    [SerializeField]
    protected StatusCalculator damageCalculator;
    [SerializeField]
    protected StatusCalculator criticalDamageCalculator;

    public UnityEvent startAttackEvent;
    public UnityEvent endAttackEvent;

    [ReadOnly]
    [SerializeField]
    protected float currentCoolTime;
    public float coolTime;

    protected virtual void Awake()
    {
        controller = GetComponent<MonsterController>();
    }

    public abstract void StartAttack(Transform target);
    protected abstract void Update();
    public abstract void EndAttack();

}
