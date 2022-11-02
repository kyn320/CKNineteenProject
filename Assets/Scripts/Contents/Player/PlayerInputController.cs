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

    //�Ͽ� ���� �Ÿ�
    [SerializeField]
    private float lockOnFindLength = 15f;

    //�Ͽ� ���� ����
    [SerializeField]
    private float lockOnAngleLimit = 15f;
    private float realLockOnAngleLimit = 10f;

    //��Ŭ���� ���� Ȯ�� ����
    [SerializeField]
    private float lockOnAngleDiffusion = 2.5f;

    [SerializeField]
    private LayerMask lockOnFindLayer;

    [SerializeField]
    UnityEvent <Vector3> LockOnMove;


    private GameObject lockOnObject = null;
    private Vector3 lockOnPoint;
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
            //TODO :: ���⼭ �����̶� �����ؼ� ����
            if(lockOnPoint != Vector3.zero)
                attackinputEvent?.Invoke(aimWorldPoint);
            else 
                attackinputEvent?.Invoke(lockOnPoint);
        }



        //�Ͽ�
            lockOnColliders = Physics.OverlapSphere(transform.position, lockOnFindLength, lockOnFindLayer);

            if (lockOnAngle.Length < lockOnColliders.Length)
                lockOnAngle = new float[lockOnColliders.Length];


            if (lockOnColliders.Length != 0)
            {
                for (int i = 0; i < lockOnColliders.Length; i++)
                {
                    Vector3 targetDir = (lockOnColliders[i].gameObject.transform.position - mainCamera.transform.position).normalized;
                    lockOnAngle[i] = Vector3.Angle(mainCamera.transform.forward, targetDir);

                    //�Ͽ����� ��� �Ҵ� �� ����
                    if (lockOnAngle[i] <= realLockOnAngleLimit
                        && (lockOnAngle[i] < Vector3.Angle(mainCamera.transform.forward, (lockOnPoint - mainCamera.transform.position).normalized)
                        || lockOnPoint == Vector3.zero))
                    {
                        lockOnObject = lockOnColliders[i].gameObject;
                    } 

                    //�Ͽ� UI��ġ ���������� ����
                    if (lockOnObject != null)
                    {
                        lockOnPoint = lockOnObject.transform.position;
                        LockOnMove?.Invoke(lockOnPoint);


                    //��ġ ���� ������ �Ѿ ��� ��ġ ����
                    if ((realLockOnAngleLimit < Vector3.Angle(mainCamera.transform.forward, (lockOnPoint - mainCamera.transform.position).normalized)))
                        {
                            lockOnObject = null;
                            LockOnMove?.Invoke(Vector3.zero);
                        }
                    }
                }
            }
            else
            {
                lockOnPoint = Vector3.zero;
                lockOnObject = null;
            LockOnMove?.Invoke(Vector3.zero);
        }
        

        if(Input.GetMouseButton(1))
        {
            realLockOnAngleLimit = lockOnAngleLimit * lockOnAngleDiffusion;
        }
        else
        {
            realLockOnAngleLimit = lockOnAngleLimit / lockOnAngleDiffusion;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Jump Input
            jumpInputEvent?.Invoke();
        }
        else
        {
            //�÷��̾� �Է°�
            inputVector.x = Input.GetAxisRaw("Horizontal");
            inputVector.z = Input.GetAxisRaw("Vertical");
            moveInputEvent?.Invoke(inputVector);
        }
    }
}
