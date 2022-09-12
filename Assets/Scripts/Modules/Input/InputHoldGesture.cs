using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InputHoldGesture : Singleton<InputHoldGesture>
{
    public float holdGestureTime;

    [SerializeField]
    private float currentHoldGestureTime;

    public float checkHoldGestureDistance;

    private Vector3 startHoldGesturePoint;

    [SerializeField]
    bool isHold = false;

    public UnityEvent startHoldEvent;
    public UnityEvent<float> holdGestureEvent;
    public UnityEvent maxHoldGestureEvent;
    public UnityEvent endHoldEvent;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Down
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                isHold = true;
                currentHoldGestureTime = 0f;
                startHoldGesturePoint = Camera.main.ScreenToViewportPoint(Input.mousePosition);
                startHoldEvent?.Invoke();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isHold)
                endHoldEvent?.Invoke();

            isHold = false;
        }

        if (isHold)
        {
            var holdCheckPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            if ((holdCheckPosition - startHoldGesturePoint).sqrMagnitude
                <= checkHoldGestureDistance * checkHoldGestureDistance)
            {
                ///Success Gesture
                currentHoldGestureTime += Time.deltaTime;
                holdGestureEvent?.Invoke(currentHoldGestureTime / holdGestureTime);

                if (currentHoldGestureTime >= holdGestureTime)
                {
                    //Success MaxHold
                    isHold = false;
                    maxHoldGestureEvent?.Invoke();
                    endHoldEvent?.Invoke();
                }
            }
        }
    }

}
