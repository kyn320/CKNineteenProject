using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurveFollow : MonoBehaviour
{
    [SerializeField]
    protected BezierCurve bezierCurve;

    public int currentIndex = 0;
    [SerializeField]
    private float currentTime;
    [SerializeField]
    private float maxTime;
    [SerializeField]
    protected float currentProgress;

    public bool useUpdateTime;
    public bool useLookAt;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 1f);
    }

    private void FixedUpdate()
    {
        UpdatePosition();
    }

    public void SetCurve(BezierCurve bezierCurve)
    {
        this.bezierCurve = bezierCurve;
        currentIndex = 0;
        currentTime = 0f;
        currentProgress = 0f;
        //RefreshMaxTime();
    }

    public void UpdatePosition()
    {
        transform.position = bezierCurve.GetPosition(currentIndex, currentProgress);
    }

    public void UpdateBezier()
    {
        currentTime += Time.fixedDeltaTime;
        currentProgress = Mathf.Clamp01(currentTime / maxTime);

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

            if (currentIndex >= bezierCurve.lineList.Count)
            {
                useUpdateTime = false;
            }
            else
            {
                //RefreshMaxTime();
            }
            currentTime = 0f;
            currentProgress = 0f;
        }
    }


}
