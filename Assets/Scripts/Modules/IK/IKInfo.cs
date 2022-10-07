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
    private bool useRotation = true;

    private bool CheckRayCast()
    {
        return Physics.Raycast(targetTransform.position + rayStartOffset, rayDirection, out raycastHit, rayDistance, rayMask);
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
            if (usePosition)
            {
                animator.SetIKPositionWeight(avatarIKGoal, weight);

                if (useTargetTransform)
                {
                    animator.SetIKPosition(avatarIKGoal, targetTransform.position);
                }
                else if (useRayCast && CheckRayCast())
                {
                    animator.SetIKPosition(avatarIKGoal, raycastHit.point + offsetPosition);
                }
            }
            if (useRotation)
            {
                animator.SetIKRotationWeight(avatarIKGoal, weight);
                if (useTargetTransform)
                {
                    animator.SetIKRotation(avatarIKGoal, targetTransform.rotation);
                }
                else if (useRayCast && CheckRayCast())
                {
                    var rayRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(targetTransform.forward, raycastHit.normal), raycastHit.normal);
                    animator.SetIKRotation(avatarIKGoal, rayRotation * offsetRotation);
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
