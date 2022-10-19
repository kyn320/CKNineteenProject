using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCurveMove : ProjectileMoveable
{
    public float height = 5f;

    public override void Shot()
    {
        base.Shot();
        rigid.velocity = CalculateTrajectoryVelocity();
    }

    protected Vector3 CalculateTrajectoryVelocity()
    {
        var velocity = endPoint - startPoint;

        velocity.y = height;

        return velocity;
    }



}
