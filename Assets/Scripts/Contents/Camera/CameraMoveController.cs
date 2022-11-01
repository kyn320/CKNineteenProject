using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraMoveController : Singleton<CameraMoveController>
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Vector3 targetOffset;

    [SerializeField]
    private GameObject mainCamera;
    [SerializeField]
    private GameObject cameraAnchor;
    [SerializeField]
    private GameObject directionObject;

    public Vector3[] cameraVector;

    public bool isZoom;
    public bool isBack;
    public float cameraAngle = 80;

    public float dampingSpeed = 10f;

    //�ٸ� ��ũ��Ʈ�� �����Ҷ� ���� ī�޶� �ٶ󺸴� ����
    public Vector3 playerObjectForwardVector;
    public UnityEvent<Vector3> updateForwardVector;

    [SerializeField]
    private ObjectTweenAnimator tweenAnimator;

    protected override void Awake()
    {
        base.Awake();

        cameraAnchor = this.gameObject;

        directionObject.transform.localPosition = new Vector3(0, mainCamera.transform.localPosition.y, mainCamera.transform.localPosition.z);

        cameraVector[0] = mainCamera.transform.localPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CameraMode();
        transform.position = Vector3.Lerp(transform.position, target.position + targetOffset, Time.deltaTime * dampingSpeed);
    }

    public void CameraMove(Vector2 mousePos)
    {
        //ī�޶� ȸ����
        Vector3 cameraRot = cameraAnchor.transform.localEulerAngles;

        //����� ���콺 ��Ŀ ȸ������ X��
        float x = cameraRot.x - mousePos.y;

        //ī�޶� ���� ����
        if (x > cameraAngle && x < 180f)
            x = cameraAngle;
        else if (x < 360f - cameraAngle && x > 180f)
            x = 360f - cameraAngle;

        //ī�޶� ��ġ�� ����
        cameraAnchor.transform.localEulerAngles = new Vector3(x, cameraRot.y + mousePos.x, cameraRot.z);
        directionObject.transform.localEulerAngles = new Vector3(x, cameraRot.y + mousePos.x, cameraRot.z);

        //�÷��̾� ������Ʈ�� �����ִ� ���� ����
        playerObjectForwardVector = cameraAnchor.transform.position - directionObject.transform.position;

        updateForwardVector?.Invoke(playerObjectForwardVector);

        //�� ���� ���� �⺻ ������ ī�޶��� ī�޶� ���� ���� �ʵ��� ī�޶� ������ ����Ѵ�.
        if (!isZoom)
            CameraWallGuarder(0);

    }

    private void CameraWallGuarder(float rayDistance)
    {
        //RayCast�� ����ؼ� ���� ī�޶� ������Ʈ�� �վ��ٸ� ī�޶� ������Ʈ �ٱ����� ������

        if (rayDistance <= 0)
            rayDistance = Vector3.Distance(cameraAnchor.transform.position, directionObject.transform.position);

        Vector3 rayVector = mainCamera.transform.position - transform.position;
        RaycastHit hit;
        Physics.Raycast(transform.position, rayVector, out hit, rayDistance);
        Debug.DrawRay(transform.position, rayVector, Color.red);

        if (hit.collider != null && hit.collider.gameObject.CompareTag("Ground"))
        {
            mainCamera.transform.position = hit.point;
        }
    }

    private void CameraMode()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            isZoom = true;
        }
        else if (Input.GetKeyUp(KeyCode.R))
        {
            isZoom = false;
        }

        UpdateCameraPosition();
    }

    private void UpdateCameraPosition()
    {
        Vector3 targetPosition;
        if (isZoom)
        {
            targetPosition = cameraVector[1];
        }
        else if (isBack)
        {
            targetPosition = cameraVector[2];
        }
        else
        {
            targetPosition = cameraVector[0];
        }
        mainCamera.transform.localPosition = Vector3.Lerp(mainCamera.transform.localPosition, targetPosition, Time.deltaTime * dampingSpeed);
    }

    public void SetBackMoveCamera(bool isBack)
    {
        this.isBack = isBack;
    }

    public void PlayTweenAnimation(List<ObjectTweenAnimationData> animations)
    {
        tweenAnimator.PlayAnimation(animations);
    }
}
