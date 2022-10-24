using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class PlayerInputController : MonoBehaviour
{
    private const float MaxAimDistance = 50f;
    [SerializeField]
    private GameObject mainCamera;

    public bool allowInput = true;

    [ReadOnly]
    [ShowInInspector]
    private Vector3 inputVector;
    public UnityEvent<Vector3> moveInputEvent;

    public UnityEvent jumpInputEvent;

    public UnityEvent<Vector2> mouseMoveEvent;

    public UnityEvent<GameObject> interactiveInputEvnet;

    public UnityEvent<Vector3> attackinputEvent;


    [ReadOnly]
    [ShowInInspector]
    private Vector3 aimWorldPoint;
    private RaycastHit aimRayCastHit;


    [Header("LockOnSetting")]
    [SerializeField]
    private Vector3 lockOnPoint;
    [SerializeField]
    private bool isLockOn;
    [SerializeField]
    private float lockOnFindLength = 50f;
    [SerializeField]
    private float lockOnDirectionLength = 50f;
    [SerializeField]
    private GameObject lockOnUI;
    [SerializeField]
    private LayerMask lockOnFindLayer;
    [SerializeField]
    GameObject lockOnObject = null;
    [SerializeField]
    private Collider[] lockOnColliders = null;
    private float[] lockOnAngle = new float[100];



    private void Update()
    {
        if (!allowInput)
        {
            inputVector = Vector3.zero;
            moveInputEvent?.Invoke(inputVector);
            return;
        }

        var mousePos = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        mouseMoveEvent?.Invoke(mousePos);

        Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out aimRayCastHit);
        Debug.DrawRay(mainCamera.transform.position, mainCamera.transform.forward * MaxAimDistance, Color.blue);

        if (aimRayCastHit.point == Vector3.zero)
            aimWorldPoint = mainCamera.transform.position + mainCamera.transform.forward * MaxAimDistance;
        else
        {
            if (aimRayCastHit.point == Vector3.zero)
                aimWorldPoint = mainCamera.transform.position + mainCamera.transform.forward * MaxAimDistance;
            else
                aimWorldPoint = aimRayCastHit.point;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (aimRayCastHit.collider != null)
            {
                var interactive = aimRayCastHit.collider.gameObject.GetComponent<IInteractable>();
                if (interactive != null)
                {
                    //Success Interactive
                    interactive.Interactive();
                    Debug.Log(aimRayCastHit.collider.gameObject.name);
                    interactiveInputEvnet?.Invoke(aimRayCastHit.collider.gameObject);
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            //TODO :: 여기서 공격이랑 연결해서 쓰기
            if(isLockOn)
            attackinputEvent?.Invoke(lockOnPoint);
            else
            attackinputEvent?.Invoke(aimWorldPoint);
        }

        //마우스 우클릭시 록온
        if (Input.GetMouseButton(1))
        {
            isLockOn = true;

            lockOnColliders = Physics.OverlapSphere(transform.position, lockOnFindLength, lockOnFindLayer);

            for (int i = 0; i < lockOnColliders.Length; i++)
            {
                Vector3 targetDir = (lockOnColliders[i].gameObject.transform.position - mainCamera.transform.position).normalized;
                lockOnAngle[i] = Vector3.Angle(transform.forward, targetDir);

                if (lockOnAngle[i] <= lockOnDirectionLength
                    && (Vector3.Angle(mainCamera.transform.forward, (lockOnPoint - mainCamera.transform.position).normalized) > lockOnAngle[i]
                    || lockOnPoint == Vector3.zero))
                {
                    lockOnObject = lockOnColliders[i].gameObject;
                }

                if (lockOnObject != null)
                {
                    lockOnPoint = lockOnObject.transform.position;
                    lockOnUI.transform.position = Camera.main.WorldToScreenPoint(lockOnPoint);
                    Debug.DrawRay(mainCamera.transform.position, lockOnPoint - mainCamera.transform.position, Color.green);
                }
                else
                {
                    lockOnPoint = Vector3.zero;
                    lockOnUI.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
                }
            }
        }
        else
        {
            isLockOn = false;

            lockOnPoint = Vector3.zero;
            lockOnUI.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        }



        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Jump Input
            jumpInputEvent?.Invoke();
        }
        else
        {
            //플레이어 입력값
            inputVector.x = Input.GetAxisRaw("Horizontal");
            inputVector.z = Input.GetAxisRaw("Vertical");
            moveInputEvent?.Invoke(inputVector);
        }
    }

}
