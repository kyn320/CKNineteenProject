using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class FieldOfView : MonoBehaviour
{
    [SerializeField]
    private bool useUpdate = false;

    [SerializeField]
    protected float viewRadius;
    [SerializeField]
    [Range(0f, 360f)]
    protected float viewAngle;

    [SerializeField]
    private LayerMask targetMask;
    [SerializeField]
    private LayerMask obstacleMask;

    public UnityEvent<List<Collider>> visibleEvent;

    [SerializeField]
    protected List<Collider> visibleTargetList;

    private Collider[] enterColliders;

    public void SetRadius(float viewRadius) {
        this.viewRadius = viewRadius;
    }
    public void SetAngle(float viewAngle)
    {
        this.viewAngle = viewAngle;
    }


    private void Update()
    {
        if (useUpdate)
        {
            FindFOV();
        }
    }

    [Button("�þ� �� Ž��")]
    public void FindFOV()
    {
        visibleTargetList.Clear();

        enterColliders = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        if (enterColliders.Length == 0)
            return;

        for (var i = 0; i < enterColliders.Length; ++i)
        {
            Transform target = enterColliders[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle * 0.5f)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    visibleTargetList.Add(enterColliders[i]);

                    Debug.DrawRay(transform.position, dirToTarget * viewRadius, Color.red);
                }
            }
        }

        if (visibleTargetList.Count > 0)
            visibleEvent?.Invoke(visibleTargetList);
    }

    public bool CheckFov(Transform target, float viewAngle, float checkDistance)
    {
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        float dstToTarget = Vector3.Distance(transform.position, target.position);

        if (dstToTarget <= checkDistance && Vector3.Angle(transform.forward, dirToTarget) < viewAngle * 0.5f)
        {
            return !Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask);
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        Vector3 myPos = transform.position;
        Gizmos.DrawWireSphere(myPos, viewRadius);

        float lookingAngle = transform.eulerAngles.y;  //ĳ���Ͱ� �ٶ󺸴� ������ ����
        Vector3 rightDir = Vector3Utility.AngleToDir(lookingAngle + viewAngle * 0.5f);
        Vector3 leftDir = Vector3Utility.AngleToDir(lookingAngle - viewAngle * 0.5f);
        Vector3 lookDir = Vector3Utility.AngleToDir(lookingAngle);

        Debug.DrawRay(myPos, rightDir * viewRadius, Color.blue);
        Debug.DrawRay(myPos, leftDir * viewRadius, Color.blue);
        Debug.DrawRay(myPos, lookDir * viewRadius, Color.cyan);
    }


}
