using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileStraightMove : ProjectileMoveable
{
    public override void Move()
    {
        base.Move();
        //���� �̵� ���� ����
        transform.position += moveDirection * moveSpeed * Time.fixedDeltaTime;
    }
}
