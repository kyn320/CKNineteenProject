using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public enum IKTarget
{
    Head,
    LeftHand,
    RightHand,
    LeftFoot,
    RightFoot,
}

[System.Serializable]
public class IKInfo
{
    [SerializeField]
    private string name;

    [SerializeField]
    private IKTarget ikTarget;
    [SerializeField]
    private float weight;

    [SerializeField]
    private bool useTargetTransform = false;
    [SerializeField]
    private Transform targetTransform;

    [SerializeField]
    private bool useRayCast = false;
    [SerializeField]
    private Vector3 rayStartOffset;
    [SerializeField]
    private Vector3 rayDirection;
    [SerializeField]
    private float rayDistance;

    [SerializeField]
    private bool useDynamicRayDistance;
    [SerializeField]
    private AmountRangeFloat dynamicRayDistanceRange;

    private RaycastHit raycastHit;
    [SerializeField]
    private LayerMask rayMask;
    [SerializeField]
    private Vector3 offsetPosition;
    [SerializeField]
    private Quaternion offsetRotation;

    [SerializeField]
    private bool usePosition = true;
    [SerializeField]
    private bool useDampingPosition = true;
    [SerializeField]
    private float dampingPosition = 10f;


    [SerializeField]
    private bool useRotation = true;
    [SerializeField]
    private bool useDampingRotation = true;
    [SerializeField]
    private float dampingRotation = 10f;

    public Vector3 rayHitPoint;
    public Vector3 rayHitNormal;

    public void UpdateDynamicRayDistance(float progress)
    {
        if (useDynamicRayDistance)
            rayDistance = Mathf.Lerp(dynamicRayDistanceRange.min, dynamicRayDistanceRange.max, progress);
    }

    private bool CheckRayCast()
    {
        return Physics.Raycast(targetTransform.position + rayStartOffset, rayDirection, out raycastHit, rayDistance, rayMask);
    }

    private bool CheckRayCast(Vector3 startRayPosition)
    {
        Debug.DrawRay(startRayPosition + rayStartOffset, rayDirection * rayDistance);
        return Physics.Raycast(startRayPosition + rayStartOffset, rayDirection, out raycastHit, rayDistance, rayMask);
    }

    public void UpdateIK(Animator animator, bool isActiveIK)
    {
        var avatarIKGoal = AvatarIKGoal.LeftHand;

        switch (ikTarget)
        {
            case IKTarget.Head:
                if (isActiveIK)
                {
                    animator.SetLookAtWeight(weight);
                    if (useTargetTransform)
                    {
                        animator.SetLookAtPosition(targetTransform.position);
                    }
                    else if (useRayCast && CheckRayCast())
                    {
                        rayHitPoint = raycastHit.point;
                        animator.SetLookAtPosition(raycastHit.point + offsetPosition);
                    }
                }
                else
                {
                    animator.SetLookAtWeight(0);
                }

                return;
            case IKTarget.LeftHand:
                avatarIKGoal = AvatarIKGoal.LeftHand;
                break;
            case IKTarget.RightHand:
                avatarIKGoal = AvatarIKGoal.RightHand;
                break;
            case IKTarget.LeftFoot:
                avatarIKGoal = AvatarIKGoal.LeftFoot;
                break;
            case IKTarget.RightFoot:
                avatarIKGoal = AvatarIKGoal.RightFoot;
                break;
        }

        if (isActiveIK)
        {
            var ikPosition = animator.GetIKPosition(avatarIKGoal);
            var ikRotation = animator.GetIKRotation(avatarIKGoal);

            if (useTargetTransform)
            {
                if (usePosition)
                {
                    animator.SetIKPositionWeight(avatarIKGoal, weight);
                    if (useDampingPosition) { 
                        ikPosition = Vector3.Slerp(ikPosition, targetTransform.position, Time.deltaTime * dampingPosition);
                    }
                    else
                    { 
                        ikPosition = targetTransform.position;
                    }
                    animator.SetIKPosition(avatarIKGoal, ikPosition);
                }
                else
                {
                    animator.SetIKPositionWeight(avatarIKGoal, 0);
                }

                if (useRotation)
                {
                    animator.SetIKRotationWeight(avatarIKGoal, weight);

                    if (useDampingRotation)
                    {
                        ikRotation = Quaternion.Slerp(ikRotation, targetTransform.rotation, Time.deltaTime * dampingRotation);
                    }
                    else
                    {
                        ikRotation = targetTransform.rotation;
                    }
                    animator.SetIKRotation(avatarIKGoal, ikRotation);
                }
                else
                {
                    animator.SetIKRotationWeight(avatarIKGoal, 0);
                }
            }
            else if (useRayCast)
            {
                if (CheckRayCast(targetTransform.position))
                {
                    if (usePosition)
                    {
                        rayHitPoint = raycastHit.point;
                        animator.SetIKPositionWeight(avatarIKGoal, weight);

                        if (useDampingPosition)
                        {
                            ikPosition = Vector3.Slerp(ikPosition, raycastHit.point + offsetPosition, Time.deltaTime * dampingPosition);
                        }
                        else
                        {
                            ikPosition = raycastHit.point + offsetPosition;
                        }
                        animator.SetIKPosition(avatarIKGoal, ikPosition);
                    }
                    else
                    {
                        animator.SetIKPositionWeight(avatarIKGoal, 0);
                    }

                    if (useRotation)
                    {
                        rayHitNormal = Vector3.ProjectOnPlane(animator.transform.forward, raycastHit.normal);
                        animator.SetIKRotationWeight(avatarIKGoal, weight);
                        var lookAtRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(animator.transform.forward, raycastHit.normal), raycastHit.normal);

                        if (useDampingRotation)
                        {
                            ikRotation = Quaternion.Slerp(ikRotation, lookAtRotation, Time.deltaTime * dampingRotation);
                        }
                        else
                        {
                            ikRotation = lookAtRotation;
                        }
                        animator.SetIKRotation(avatarIKGoal, ikRotation);
                    }
                    else
                    {
                        animator.SetIKRotationWeight(avatarIKGoal, 0);
                    }
                }
                else
                {
                    if (usePosition)
                    {
                        animator.SetIKPositionWeight(avatarIKGoal, 0);
                    }

                    if (useRotation)
                    {
                        animator.SetIKRotationWeight(avatarIKGoal, 0);
                    }
                }
            }
            else
            {
                if (usePosition)
                {
                    animator.SetIKPositionWeight(avatarIKGoal, 0);
                }

                if (useRotation)
                {
                    animator.SetIKRotationWeight(avatarIKGoal, 0);
                }
            }
        }
        else
        {
            if (usePosition)
            {
                animator.SetIKPositionWeight(avatarIKGoal, 0);
            }

            if (useRotation)
            {
                animator.SetIKRotationWeight(avatarIKGoal, 0);
            }
        }
    }

}
