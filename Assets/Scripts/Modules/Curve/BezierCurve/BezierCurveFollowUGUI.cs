using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class BezierCurveFollowUGUI : MonoBehaviour
{
    [SerializeField]
    protected BezierCurveUGUI bezierCurve;

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
    private RectTransform foregroundArea;
    [SerializeField]
    private RectTransform backgroundArea;
    [SerializeField]
    private RectTransform orderPivot;
    public bool useAutoSortingOrder = true;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.25f);
    }

    private void Start()
    {
        if (bezierCurve != null)
            timePerCount = maxTime / bezierCurve.GetLineCount();
    }

    private void FixedUpdate()
    {
        UpdatePosition();
        UpdateBezier();
    }

    public void SetCurve(BezierCurveUGUI bezierCurve)
    {
        this.bezierCurve = bezierCurve;
        timePerCount = maxTime / bezierCurve.GetLineCount();
    }

    public void UpdatePosition()
    {
        transform.position = bezierCurve.GetPosition(currentIndex, currentProgress);

        if (useAutoSortingOrder)
        {
            if (transform.position.y < orderPivot.position.y)
            {
                transform.SetParent(foregroundArea);
            }
            else
            {
                transform.SetParent(backgroundArea);
            }
        }
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
