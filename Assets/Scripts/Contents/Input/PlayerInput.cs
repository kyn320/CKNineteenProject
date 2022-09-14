using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
    private Vector3 moveDirection;
    public UnityEvent<Vector3> moveEvent;
    public UnityEvent jumpEvent;


    private Vector2 mousePosition;
    [SerializeField]
    protected float mouseSensitivity;
    public UnityEvent<Vector2> mouseMoveEvent;

    [SerializeField]
    bool isMouseHold = false;
    public UnityEvent mouseDownEvent;
    [SerializeField]
    private float mouseHoldTime;
    public UnityEvent<float> mouseHoldEvent;
    public UnityEvent mouseUpEvent;

    //마우스 기반 캐릭터 회전, ( 그러면 카메라도 같이 회전됨! 와!)

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isMouseHold = true;
            mouseDownEvent?.Invoke();
        }

        if (Input.GetMouseButtonUp(0))
        {
            isMouseHold = false;
            mouseUpEvent?.Invoke();
        }

        if (isMouseHold)
        {
            mouseHoldTime += Time.deltaTime;
            mouseHoldEvent?.Invoke(mouseHoldTime);
        }

        moveDirection.x = Input.GetAxis("Horizontal");
        moveDirection.z = Input.GetAxis("Vertical");

        moveEvent?.Invoke(moveDirection);

        mousePosition.x = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
        mousePosition.y = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

        mouseMoveEvent?.Invoke(mousePosition);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpEvent?.Invoke();
        }

    }


}
