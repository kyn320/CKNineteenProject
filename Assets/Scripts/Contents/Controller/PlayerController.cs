using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{

    private new Rigidbody rigidbody;

    [SerializeField]
    protected float moveSpeed;
    [SerializeField]
    protected float rotateSpeed;
    [SerializeField]
    protected float jumpPower;

    [SerializeField]
    protected Vector3 maxVelocity;

    [SerializeField]
    private bool isGrounded = false;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 moveDirection)
    {
        if (!isGrounded)
            return;

        var fowardDirection = transform.forward * moveDirection.z;
        var rightDirection = transform.right * moveDirection.x;

        var velocity = (fowardDirection + rightDirection).normalized * moveSpeed * Time.fixedDeltaTime;

        velocity.y = rigidbody.velocity.y;
        rigidbody.velocity = velocity;
    }

    public void Rotate(Vector2 mouseMovePosition)
    {
        rigidbody.MoveRotation(rigidbody.rotation * Quaternion.Euler(0f, mouseMovePosition.x, 0f));
    }

    public void Jump()
    {
        if(!isGrounded)
            return;

        var velocity = rigidbody.velocity;
        velocity.y = jumpPower;

        rigidbody.velocity = velocity;
        isGrounded = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }


    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

}
