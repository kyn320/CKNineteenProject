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
        fieldOfView.SetRadius(controller.GetStatus().currentStatus.GetElement(StatusType.SightDistance).CalculateTotalAmount());
        fieldOfView.SetAngle(controller.GetStatus().currentStatus.GetElement(StatusType.SightDegree).CalculateTotalAmount());

        for (var i = 0; i < enterAnimatorTriggerList.Count; ++i)
        {
            enterAnimatorTriggerList[i].Invoke(controller.GetAnimator());
        }

        fieldOfView.enabled = true;
        fieldOfView.visibleEvent.AddListener(EnterSight);

        enterEvent?.Invoke();
    }

    public override void Update()
    {
        return;
    }

    public override void Exit()
    {
        fieldOfView.visibleEvent.RemoveListener(EnterSight);
        fieldOfView.enabled = false;

        exitEvent?.Invoke();
    }

    public void EnterSight(List<Collider> enterSightList)
    {
        if (enterSightList.Count > 0)
        {
            controller.SetTarget(enterSightList[0].transform);
            controller.ChangeState(MonsterStateType.MONSTERSTATE_CHASE);
        }
    }

}

