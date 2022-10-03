using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class PlayerInputController : MonoBehaviour
{
    private const float MaxAimDistance = 50000f;
    [SerializeField]
    private GameObject mainCamera;

    public bool allowInput = true;

    private Vector3 inputVector;
    public UnityEvent<Vector3> moveInputEvent;

    public UnityEvent jumpInputEvent;

    public UnityEvent<GameObject> interactiveInputEvnet;

    public UnityEvent<Vector3> attackinputEvent;

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

        Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out aimRayCastHit);
        Debug.DrawRay(mainCamera.transform.position, mainCamera.transform.forward * MaxAimDistance, Color.blue);

        if (aimRayCastHit.point == Vector3.zero)
            aimWorldPoint = mainCamera.transform.position + mainCamera.transform.forward * MaxAimDistance;
        else
            aimWorldPoint = aimRayCastHit.point;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if(aimRayCastHit.collider != null) { 
                var interactive = aimRayCastHit.collider.gameObject.GetComponent<IInteractable>();
                if(interactive != null) {
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
            attackinputEvent?.Invoke(aimWorldPoint);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Jump Input
            jumpInputEvent?.Invoke();
        }
        else
        {
            //�÷��̾� �Է°�
            inputVector.x = Input.GetAxis("Vertical");
            inputVector.z = Input.GetAxis("Horizontal");
            moveInputEvent?.Invoke(inputVector);
        }
    }


}
