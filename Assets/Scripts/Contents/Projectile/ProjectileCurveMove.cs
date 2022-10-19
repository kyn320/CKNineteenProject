using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCurveMove : ProjectileMoveable
{
    new Rigidbody rigidbody;
    public float height = 5f;

    public override void Shot()
    {
        base.Shot();
        rigid.useGravity = true;
        rigid.isKinematic = false;
        rigid.velocity = CalculateTrajectoryVelocity();
    }

    protected Vector3 CalculateTrajectoryVelocity()
    {
        var velocity = endPoint - startPoint;

        velocity.y = height;

        return velocity;
    }



}
