using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(Animator))]
public class IKController : MonoBehaviour
{
    public SerializableDictionary<IKTarget, IKInfo> ikInfoDic = new SerializableDictionary<IKTarget, IKInfo>();

    private Animator animator;

    public bool isActiveIK = true;

    [SerializeField]
    private float gizmoSize = 0.25f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void OnAnimatorIK()
    {
        if (animator == null)
            return;

        foreach (var ikInfo in ikInfoDic.Values)
        {
            ikInfo.UpdateIK(animator, isActiveIK);
        }
    }

    public void UpdateDynamicRayDistance(float progress)
    {
        foreach (var ikInfo in ikInfoDic.Values)
        {
            ikInfo.UpdateDynamicRayDistance(progress);
        }
    }

    private void OnDrawGizmos()
    {
        if (animator == null)
            return;

        foreach (var ikInfo in ikInfoDic.Values)
        {
            Gizmos.DrawSphere(ikInfo.rayHitPoint, gizmoSize);
            Gizmos.DrawLine(ikInfo.rayHitPoint, ikInfo.rayHitPoint + ikInfo.rayHitNormal);
        }
    }

}
