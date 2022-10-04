using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterIdleState : MonsterStateBase
{
    [SerializeField]
    private FieldOfView fieldOfView;

    protected override void Awake()
    {
        base.Awake();
        fieldOfView = GetComponent<FieldOfView>();  
    }

    public override void Enter()
    {
        fieldOfView.enabled = true;
        fieldOfView.visibleEvent.AddListener(EnterSight);
    }

    public override void Update()
    {
        return;
    }

    public override void Exit()
    {
        fieldOfView.visibleEvent.RemoveListener(EnterSight);
        fieldOfView.enabled = false;
    }

    public void EnterSight(List<Collider> enterSightList)
    {
        if(enterSightList.Count > 0) { 
            controller.SetTarget(enterSightList[0].transform);
            controller.ChangeState(MonsterStateType.MONSTERSTATE_CHASE);
        }
    }

}

