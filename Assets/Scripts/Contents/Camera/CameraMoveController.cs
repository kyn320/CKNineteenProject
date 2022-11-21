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

    //다른 스크립트에 참고할때 사용될 카메라가 바라보는 방향
    public Vector3 playerObjectForwardVector;
    public UnityEvent<Vector3> updateForwardVector;

    [SerializeField]
    private ObjectTweenAnimator tweenAnimator;

    protected override void Awake()
    {
        base.Awake();

        cameraAnchor = this.gameObject;

        directionObject.transform.localPosition = new Vector3(0, mainCamera.transform.localPosition.y, mainCamera.transform.localPosition.z);

        //cameraVector[0] = mainCamera.transform.localPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + targetOffset, Time.deltaTime * dampingSpeed);
    }

    private void Update()
    {
        CameraMode();
    }

    public void CameraMove(Vector2 mousePos)
    {
        //카메라 회전값
        Vector3 cameraRot = cameraAnchor.transform.localEulerAngles;

        //변경될 마우스 엥커 회전값의 X값
        float x = cameraRot.x - mousePos.y;

        //카메라 상하 제한
        if (x > cameraAngle && x < 180f)
            x = cameraAngle;
        else if (x < 360f - cameraAngle && x > 180f)
            x = 360f - cameraAngle;

        //카메라 위치값 적용
        cameraAnchor.transform.localEulerAngles = new Vector3(x, cameraRot.y + mousePos.x, cameraRot.z);
        directionObject.transform.localEulerAngles = new Vector3(x, cameraRot.y + mousePos.x, cameraRot.z);

        //플레이어 오브젝트가 보고있는 앞쪽 백터
        playerObjectForwardVector = cameraAnchor.transform.position - directionObject.transform.position;

        updateForwardVector?.Invoke(playerObjectForwardVector);

        //줌 되지 않은 기본 상태의 카메라라면 카메라가 벽을 넘지 않도록 카메라 가더를 사용한다.
        if (!isZoom)
            CameraWallGuarder(0);

    }

    private void CameraWallGuarder(float rayDistance)
    {
        //RayCast를 사용해서 만약 카메라가 오브젝트를 뚫었다면 카메라를 오브젝트 바깥으로 가저옴

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
