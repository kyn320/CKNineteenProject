using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHitState : MonsterStateBase
{
    private Rigidbody rigid;

    protected override void Awake()
    {
        base.Awake();
        rigid = GetComponent<Rigidbody>();
    }

    // 넉백 효과 적용
    public override void Enter()
    {
        rigid.velocity = new Vector3(transform.localPosition.x - 1, transform.localPosition.y, transform.localPosition.z - 1);
        controller.ChangeState(MonsterStateType.MONSTERSTATE_CHASE);

        enterEvent?.Invoke();
    }

    public override void Exit()
    {
        exitEvent?.Invoke();
    }

    public override void Update()
    {


    }
}
