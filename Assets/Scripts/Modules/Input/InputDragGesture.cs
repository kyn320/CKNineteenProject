using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InputDragGesture : Singleton<InputDragGesture>
{
    public float dragGestureTime;

    [SerializeField]
    private float currentDragGestureTime;

    public float checkDragGestureDistance;

    [SerializeField]
    private Vector3 startDragGesturePoint;

    [SerializeField]
    private Vector3 endDragGesturePoint;

    [SerializeField]
    private Vector3 dragGesture;

    [SerializeField]
    bool isDrag = false;

    public UnityEvent<Vector3> startDragEvent;
    public UnityEvent<Vector3> dragGestureEvent;
    public UnityEvent<Vector3> endDragEvent;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Down
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                currentDragGestureTime = dragGestureTime;
                startDragGesturePoint = Camera.main.ScreenToViewportPoint(Input.mousePosition);
                isDrag = true;
                startDragEvent?.Invoke(Input.mousePosition);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isDrag)
                endDragEvent?.Invoke(Input.mousePosition);

            endDragGesturePoint = Vector3.zero;
            isDrag = false;
        }

        if (isDrag && currentDragGestureTime >= 0)
        {
            currentDragGestureTime -= Time.deltaTime;
            endDragGesturePoint = Camera.main.ScreenToViewportPoint(Input.mousePosition);

            if ((endDragGesturePoint - startDragGesturePoint).sqrMagnitude
                >= checkDragGestureDistance * checkDragGestureDistance)
            {
                dragGesture = endDragGesturePoint - startDragGesturePoint;
                dragGesture.Normalize();
                dragGestureEvent?.Invoke(dragGesture);
                isDrag = false;
                endDragEvent?.Invoke(Input.mousePosition);
            }
        }
    }

}
