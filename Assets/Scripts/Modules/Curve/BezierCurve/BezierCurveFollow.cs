using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[ExecuteInEditMode]
public class BezierCurveFollow : MonoBehaviour
{
    [SerializeField]
    protected BezierCurve bezierCurve;

    public int currentIndex = 0;
    [SerializeField]
    private float currentTime;
    [ReadOnly]
    [ShowInInspector]
    private float timePerCount;
    [SerializeField]
    private float maxTime;
    [SerializeField]
    protected float currentProgress;

    public bool useUpdateTime;
    public bool useLookAt;
    public bool isLoop = false;

    [SerializeField]
    private float gizmoRadius = 0.1f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, gizmoRadius);
    }

    private void Start()
    {
        if (bezierCurve != null)
            timePerCount = maxTime / bezierCurve.GetLineCount();
    }

    //TEST
    private void Update()
    {
        transform.position = bezierCurve.GetPosition(currentIndex, currentProgress);
    }
    //TEST

    private void FixedUpdate()
    {
        UpdatePosition();
        UpdateBezier();
    }

    public void SetCurve(BezierCurve bezierCurve)
    {
        this.bezierCurve = bezierCurve;
        timePerCount = maxTime / bezierCurve.GetLineCount();
    }

    public void UpdatePosition()
    {
        transform.position = bezierCurve.GetPosition(currentIndex, currentProgress);
    }

    public void UpdateBezier()
    {
        currentTime += Time.fixedDeltaTime;
        currentProgress = Mathf.Clamp01(currentTime / timePerCount);

        var nextPosition = bezierCurve.GetPosition(currentIndex, currentProgress);

        if (useLookAt)
        {
            var lookAtPosition = nextPosition - transform.position;
            lookAtPosition.Normalize();
            var lookAtDegree = Mathf.Atan2(lookAtPosition.x, lookAtPosition.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, lookAtDegree, 0f);
        }

        transform.position = nextPosition;

        if (currentProgress >= 1f)
        {
            ++currentIndex;

            if (currentIndex >= bezierCurve.GetLineCount())
            {
                if (isLoop)
                {
                    useUpdateTime = true;
                    currentIndex = 0;
                }
                else
                {
                    useUpdateTime = false;
                }
            }
            currentTime = 0f;
            currentProgress = 0f;
        }
    }


}
