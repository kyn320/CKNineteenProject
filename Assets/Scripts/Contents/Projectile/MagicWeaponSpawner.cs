using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class MagicWeaponSpawner : MonoBehaviour
{
    [ShowInInspector]
    [ReadOnly]
    private Transform startPoint;
    [ShowInInspector]
    [ReadOnly]
    private Transform endPoint;

    [SerializeField]
    private float moveTime;
    [ShowInInspector]
    [ReadOnly]
    private float currentMoveTime;

    [ShowInInspector]
    [ReadOnly]
    private bool isMove = false;

    public UnityEvent destinationEvent;

    public void SetMovePoints(Transform startPoint, Transform endPoint, float moveTime)
    {
        currentMoveTime = 0f;
        this.moveTime = moveTime;
        this.startPoint = startPoint;
        this.endPoint = endPoint;
        isMove = true;
    }

    private void Update()
    {
        if (!isMove || startPoint == null || endPoint == null)
            return;

        currentMoveTime += Time.deltaTime;

        var lerpTime = currentMoveTime / moveTime;
        lerpTime = Mathf.Clamp01(lerpTime);

        transform.position = Vector3.Lerp(startPoint.position, endPoint.position, lerpTime);

        if (lerpTime >= 1f)
        {
            isMove = false;
            destinationEvent?.Invoke();
        }
    }

}
