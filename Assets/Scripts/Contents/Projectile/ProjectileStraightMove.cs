using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileStraightMove : ProjectileMoveable
{
    public override void Move()
    {
        base.Move();
        //직선 이동 로직 구현
        transform.position += moveDirection * moveSpeed * Time.fixedDeltaTime;
    }
}
