using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMoveable : MonoBehaviour
{
    [SerializeField]
    private ProjectileController projectileController;

    protected Rigidbody rigid;

    [SerializeField]
    private Vector3 startPoint;

    [SerializeField]
    protected float maxDistance;

    [SerializeField]
    protected Vector3 moveDirection;

    [SerializeField]
    protected float moveSpeed;


    protected void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }
    public virtual void SetStartPoint(Vector3 startPoint)
    {
        this.startPoint = startPoint;
    }

    public virtual void SetDirection(Vector3 moveDirection)
    {
        this.moveDirection = moveDirection;
    }

    public virtual void SetMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }

    public virtual void SetMaxDistance(float maxDistance)
    {
        this.maxDistance = maxDistance;
    }

    public virtual void Move()
    {
        if (GetCurrentDistance() >= maxDistance)
        {
            Debug.Log($"{GetCurrentDistance()} / {maxDistance}");
            //TODO :: 최대 사거리를 넘어서면, 자동으로 삭제 해주세요.
            gameObject.SetActive(false);
        }
    }

    public float GetCurrentDistance()
    {
        return Vector3.Distance(startPoint, transform.position);
    }

}
