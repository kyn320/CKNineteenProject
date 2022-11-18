using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityScaler : MonoBehaviour
{
    private Rigidbody rigid;
    public float gravityScale = 1f;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        rigid.useGravity = false;
    }

    private void FixedUpdate()
    {
        rigid.AddForce(Physics.gravity * gravityScale, ForceMode.Acceleration);
    }
}
