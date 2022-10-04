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

    private void Update()
    {
        if (useUpdate)
        {
            FindFOV();
        }
    }

    [Button("시야 내 탐색")]
    public void FindFOV()
    {
        visibleTargetList.Clear();

        enterColliders = Physics.OverlapSphere(transform.position,viewRadius, targetMask);

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

    private void OnDrawGizmos()
    {
        Vector3 myPos = transform.position;
        Gizmos.DrawWireSphere(myPos, viewRadius);

        float lookingAngle = transform.eulerAngles.y;  //캐릭터가 바라보는 방향의 각도
        Vector3 rightDir = Vector3Utility.AngleToDir(lookingAngle + viewAngle * 0.5f);
        Vector3 leftDir = Vector3Utility.AngleToDir(lookingAngle - viewAngle * 0.5f);
        Vector3 lookDir = Vector3Utility.AngleToDir(lookingAngle);

        Debug.DrawRay(myPos, rightDir * viewRadius, Color.blue);
        Debug.DrawRay(myPos, leftDir * viewRadius, Color.blue);
        Debug.DrawRay(myPos, lookDir * viewRadius, Color.cyan);
    } 
    

}
