using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwingTrap : MonoBehaviour
{
    public float swingTime;
    private float currentSwingTime;

    public int startAxis;
    private int currentAxis;

    [Range(0f, 360f)]
    public float swingDegree;

    private Quaternion startSwingRotation;
    private Quaternion targetSwingRotation;

    [SerializeField]
    private AnimationCurve animationCurve;

    [SerializeField]
    private bool isUpdate = true;

    private void Start()
    {
        currentAxis = startAxis;
        startSwingRotation = Quaternion.Euler(0, 0, swingDegree * 0.5f * startAxis);
        ChangeSwingTarget();
    }

    private void Update()
    {
        if (!isUpdate)
            return;

        Swing();
    }

    protected void Swing()
    {
        currentSwingTime += Time.deltaTime;
        var lerpTime = currentSwingTime / swingTime;
        transform.rotation = Quaternion.Lerp(startSwingRotation, targetSwingRotation, animationCurve.Evaluate(lerpTime));
        if (lerpTime >= 1)
        {
            currentSwingTime = 0f;
            ChangeSwingTarget();
        }
    }

    protected void ChangeSwingTarget()
    {
        startSwingRotation = Quaternion.Euler(0, 0, swingDegree * 0.5f * currentAxis);
        currentAxis = -currentAxis;
        targetSwingRotation = Quaternion.Euler(0, 0, swingDegree * 0.5f * currentAxis);
    }

    private void OnDrawGizmos()
    {
        Vector3 myPos = transform.position;

        var bottomLookAt = Quaternion.LookRotation(-transform.up, transform.forward);

        float lookingAngle = transform.eulerAngles.z;  //캐릭터가 바라보는 방향의 각도
        Vector3 rightDir = Vector3Utility.AngleToDir(lookingAngle + swingDegree * 0.5f);
        Vector3 leftDir = Vector3Utility.AngleToDir(lookingAngle - swingDegree * 0.5f);
        Vector3 lookDir = Vector3Utility.AngleToDir(lookingAngle);

        Debug.DrawRay(myPos, bottomLookAt * rightDir * 1f, Color.blue);
        Debug.DrawRay(myPos, bottomLookAt * leftDir * 1f, Color.blue);
        Debug.DrawRay(myPos, bottomLookAt * lookDir * 1f, Color.cyan);
    }
}
