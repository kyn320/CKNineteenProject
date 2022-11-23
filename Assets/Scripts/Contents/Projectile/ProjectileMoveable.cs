using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMoveable : MonoBehaviour
{

    [SerializeField]
    private ProjectileController projectileController;

    protected Rigidbody rigid;

    [SerializeField]
    protected Vector3 startPoint;

    [SerializeField]
    protected Vector3 endPoint;

    [SerializeField]
    protected float maxDistance;

    [SerializeField]
    protected Vector3 moveDirection;

    [SerializeField]
    protected float moveSpeed;
    [SerializeField]
    protected float moveTime;

    [SerializeField]
    protected Quaternion offsetRotation;

    protected void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }
    public virtual void SetStartPoint(Vector3 startPoint)
    {
        this.startPoint = startPoint;
        transform.position = startPoint;
    }
    public virtual void SetEndPoint(Vector3 endPoint)
    {
        this.endPoint = endPoint;
    }

    public virtual void SetDirection(Vector3 moveDirection)
    {
        this.moveDirection = moveDirection;
        transform.forward = offsetRotation * moveDirection;
    }

    public virtual void SetMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }

    public virtual void SetMaxDistance(float maxDistance)
    {
        this.maxDistance = maxDistance;
    }

    public virtual void Shot() {
        rigid = GetComponent<Rigidbody>();
    }

    public virtual void Move()
    {
        if (GetCurrentDistance() >= maxDistance)
        {
            //TODO :: �ִ� ��Ÿ��� �Ѿ��, �ڵ����� ���� ���ּ���.
            gameObject.SetActive(false);
        }
    }

    public float GetCurrentDistance()
    {
        return Vector3.Distance(startPoint, transform.position);
    }

}
