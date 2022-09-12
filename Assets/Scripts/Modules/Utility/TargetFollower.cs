using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFollower : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    [SerializeField]
    protected bool useLerp = true;
    [SerializeField]
    protected float lerpDamping = 0.5f;

    private void FixedUpdate()
    {
        if (target != null)
        {
            var targetPos = target.position + offset;
            if (useLerp) { 
                transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * lerpDamping);
            }
            else
            {
                transform.position = targetPos;
            }
        }
    }
}
