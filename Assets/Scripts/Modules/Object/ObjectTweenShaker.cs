using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class ObjectTweenShaker : MonoBehaviour
{
    public enum CycleType
    {
        Unset,
        Sin,
        Cos,
    }

    [SerializeField]
    protected CycleType cycleType;

    [SerializeField]
    private float cycleOffset;
    [SerializeField]
    [Range(0.01f, 10f)]
    private float cyclePeriod;

    [SerializeField]
    protected AnimationCurve animationCurve;

    private Vector3 originPosition;
    private Quaternion originRotation;
    private Vector3 originScale;

    [SerializeField]
    private Vector3 randomPositionRangeByPivot;
    [SerializeField]
    private Vector3 randomRotaionRangeByAxis;
    [SerializeField]
    private Vector3 randomScaleRange;

    [SerializeField]
    private bool autoActiveByPlay = false;
    [SerializeField]
    private bool autoStartPlay = false;
    [SerializeField]
    private bool resetOriginTransformByStop = false;

    [SerializeField]
    private float animationTime;

    [SerializeField]
    private UnityEvent completeEvent;

    [SerializeField]
    private UnityEvent stopEvent;

    private Coroutine animatoinCoroutine;

    private void Start()
    {
        if (autoStartPlay)
            PlayAnimation();
    }

    [Button("Àç»ý")]
    public void PlayAnimation()
    {
        if (resetOriginTransformByStop)
        {
            originPosition = transform.position;
            originRotation = transform.localRotation;
            originScale = transform.localScale;
        }

        if (autoActiveByPlay)
            gameObject.SetActive(true);

        if (animatoinCoroutine != null)
        {
            StopCoroutine(animatoinCoroutine);
        }

        animatoinCoroutine = StartCoroutine(CoShakeAnimate());
    }

    public void Stop()
    {
        if (animatoinCoroutine != null)
        {
            StopCoroutine(animatoinCoroutine);
        }

        if (resetOriginTransformByStop)
        {
            ResetOrigin();
        }

        stopEvent?.Invoke();
    }

    private void ResetOrigin()
    {
        transform.position = originPosition;
        transform.localRotation = originRotation;
        transform.localScale = originScale;
    }

    public Vector3 GetRandomVectorRange(Vector3 rangeVector)
    {
        return new Vector3(Random.Range(-rangeVector.x * 0.5f, rangeVector.x * 0.5f)
            , Random.Range(-rangeVector.y * 0.5f, rangeVector.y * 0.5f)
            , Random.Range(-rangeVector.z * 0.5f, rangeVector.z * 0.5f));
    }

    IEnumerator CoShakeAnimate()
    {
        var currentTime = 0f;

        while (true)
        {
            currentTime += Time.deltaTime;
            var lerpTime = currentTime / animationTime;

            var lerpValue = 0f;

            switch (cycleType)
            {
                case CycleType.Unset:
                    lerpValue = animationCurve.Evaluate(lerpTime);
                    break;
                case CycleType.Sin:
                    lerpValue = Mathf.Sin(lerpTime * cyclePeriod + cycleOffset);
                    break;
                case CycleType.Cos:
                    lerpValue = Mathf.Cos(lerpTime * cyclePeriod + cycleOffset);
                    break;
            }

            Debug.Log(lerpValue);

            transform.position = originPosition + randomPositionRangeByPivot * 0.5f * lerpValue;

            var randomRotationVector = randomRotaionRangeByAxis * 0.5f * lerpValue;
            var randomRotationQuaternion = Quaternion.Euler(randomRotationVector.x, randomRotationVector.y, randomRotationVector.z);
            transform.localRotation = originRotation * randomRotationQuaternion;

            transform.localScale = originScale + randomScaleRange * 0.5f * lerpValue;

            if (lerpTime > 1f)
            {
                break;
            }

            yield return null;
        }


        if (resetOriginTransformByStop)
        {
            ResetOrigin();
        }



    }

}
