using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCycleMove : ProjectileMoveable
{
    enum CycleType
    {
        None,
        Sin,
        Cos,
    }

    [SerializeField]
    private CycleType cycleType = CycleType.None;

    [SerializeField]
    private float cycleTime = 0f;

    [SerializeField]
    private int absolute = 1;
    [SerializeField]
    private float offset = 0f;
    [SerializeField]
    private float prieod = 1f;
    [SerializeField]
    private float rangeOffset = 1f;

    [SerializeField]
    private Vector3 rightVector;

    public override void SetDirection(Vector3 moveDirection)
    {
        base.SetDirection(moveDirection);
        rightVector = Quaternion.Euler(0f, 90, 0f) * moveDirection;
        rightVector.Normalize();
    }

    public override void Move()
    {
        base.Move();
        //직선 이동 로직 구현
        cycleTime += Time.fixedDeltaTime;

        switch (cycleType)
        {
            case CycleType.None:
                break;
            case CycleType.Sin:
                transform.position += absolute * rightVector * Mathf.Sin(cycleTime * prieod + offset) * rangeOffset;
                break;
            case CycleType.Cos:
                transform.position += absolute * rightVector * Mathf.Cos(cycleTime * prieod + offset) * rangeOffset;
                break;
        }

        transform.position += moveDirection * moveSpeed * Time.fixedDeltaTime;

    }
}
