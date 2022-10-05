using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class PlayerController : MonoBehaviour, IDamageable
{
    [SerializeField]
    private UnitStatus status;

    private const float MaxAimDistance = 50000f;
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

    public UnityEvent damageEvent;

    [ReadOnly]
    [ShowInInspector]
    private Vector3 aimWorldPoint;

    private RaycastHit aimRayCastHit;

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
            attackinputEvent?.Invoke(aimWorldPoint);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Jump Input
            jumpInputEvent?.Invoke();
        }
        else
        {
            //플레이어 입력값
            inputVector.x = Input.GetAxis("Horizontal");
            inputVector.z = Input.GetAxis("Vertical");
            moveInputEvent?.Invoke(inputVector);
        }
    }

    public bool OnDamage(DamageInfo damageInfo, Vector3 hitPoint, Vector3 hitNormal)
    {
        status.OnDamage(damageInfo.damage);
        damageEvent?.Invoke();
        return true;
    }
}
