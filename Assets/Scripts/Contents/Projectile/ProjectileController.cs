using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private new Rigidbody rigidbody;

    public float moveSpeed = 10f;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public void Shot(Vector3 shotPosition,Vector3 moveDirection) { 
        transform.position = shotPosition;
        transform.forward = moveDirection;

        rigidbody.velocity = transform.forward * moveSpeed;
    }

}
