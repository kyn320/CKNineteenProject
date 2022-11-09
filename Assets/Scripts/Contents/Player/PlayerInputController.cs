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

    [SerializeField]
    private float mouseDPI = 1f;

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

    //록온 가능 거리
    [SerializeField]
    private float lockOnFindLength = 15f;

    //록온 각도 범위
    [SerializeField]
    private float lockOnAngleLimit = 15f;
    [SerializeField]
    private float realLockOnAngleLimit = 15f;

    //우클릭시 범위 확산 배율
    [SerializeField]
    private float lockOnAngleDiffusion = 2.5f;

    [SerializeField]
    private LayerMask lockOnFindLayer;

    //UI에 옯겨서 사용해주세요
    [SerializeField]
    UnityEvent<Vector3> LockOnMoveEvent;

    [SerializeField]
    private Vector3 lockOnPoint;
    private GameObject lockOnObject = null;
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
        mouseMoveEvent?.Invoke(mousePos * mouseDPI);

        Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out aimRayCastHit);
        Debug.DrawRay(mainCamera.transform.position, mainCamera.transform.forward * MaxAimDistance, Color.blue);

            if (aimRayCastHit.point == Vector3.zero)
                aimWorldPoint = mainCamera.transform.position + mainCamera.transform.forward * MaxAimDistance;
            else
                aimWorldPoint = aimRayCastHit.point;
        

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
            if (lockOnPoint == new Vector3(0,0,0))
            {
                attackinputEvent?.Invoke(aimWorldPoint);
                Debug.Log($"aimWorldPoint : {aimWorldPoint}");
            }
            else
            {
                attackinputEvent?.Invoke(lockOnPoint);
                Debug.Log($"lockOnPoint : {lockOnPoint}");
            }
        }



        //록온
        lockOnColliders = Physics.OverlapSphere(transform.position, lockOnFindLength, lockOnFindLayer);

        if (lockOnAngle.Length < lockOnColliders.Length)
            lockOnAngle = new float[lockOnColliders.Length];


        if (lockOnColliders.Length != 0)
        {
            for (int i = 0; i < lockOnColliders.Length; i++)
            {
                Vector3 targetDir = (lockOnColliders[i].gameObject.transform.position - mainCamera.transform.position).normalized;
                lockOnAngle[i] = Vector3.Angle(mainCamera.transform.forward, targetDir);

                //록온중인 상대 할당 및 변경
                if (lockOnAngle[i] <= realLockOnAngleLimit
                    && (lockOnAngle[i] < Vector3.Angle(mainCamera.transform.forward, (lockOnPoint - mainCamera.transform.position).normalized)
                    || lockOnPoint == Vector3.zero))
                {
                    lockOnObject = lockOnColliders[i].gameObject;
                }

                //록온된 오브젝트가 있다면 UI위치 지속적으로 변경
                if (lockOnObject != null)
                {
                    lockOnPoint = lockOnObject.transform.position;
                    LockOnMoveEvent?.Invoke(lockOnPoint);


                    //서치 가능 제한을 넘어설 경우 서치 해제
                    if ((realLockOnAngleLimit < Vector3.Angle(mainCamera.transform.forward, (lockOnPoint - mainCamera.transform.position).normalized)))
                    {
                        lockOnPoint = Vector3.zero;
                        lockOnObject = null;
                        LockOnMoveEvent?.Invoke(Vector3.zero);
                    }
                }
            }
        }
        else
        {
            lockOnPoint = Vector3.zero;
            lockOnObject = null;
            LockOnMoveEvent?.Invoke(Vector3.zero);
        }


        if (Input.GetMouseButton(1))
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
            //플레이어 입력값
            inputVector.x = Input.GetAxisRaw("Horizontal");
            inputVector.z = Input.GetAxisRaw("Vertical");
            moveInputEvent?.Invoke(inputVector);
        }
    }
}
