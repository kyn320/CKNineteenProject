using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveController : MonoBehaviour
{

    private GameObject mainCamera;
    private GameObject cameraAnchor;
    private GameObject directionObject;
    public Vector3[] cameraVector;
    public int cameraMode;
    public float cameraAngle = 80;
    public bool cameraWallDown = false;

    //다른 스크립트에 참고할때 사용될 카메라가 바라보는 방향
    public Vector3 playerObjectForwardVector;


    // Start is called before the first frame update
    void Start()
    {
        if(cameraAnchor == null)
        { 
            cameraAnchor = this.gameObject; 
        }

        if (mainCamera == null)
        {
            mainCamera = transform.Find("Main Camera").gameObject;
            if (mainCamera == null)
            {
                Debug.LogError($"{this.gameObject.name} has no Camera...");
            }
        }

        if (directionObject == null)
        {
            directionObject = transform.Find("DirectionObject").gameObject;
            if (mainCamera == null)
            {
                Debug.LogError($"{this.gameObject.name} has no directionObject...");
                directionObject = new GameObject();
            }
        }
        directionObject.transform.localPosition = new Vector3(0, mainCamera.transform.localPosition.y, mainCamera.transform.localPosition.z);

        cameraVector[0] = mainCamera.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        CameraMove();
        CameraMode();
    }

    private void CameraMove()
    {
        

        //마우스 포인트 위치
        Vector2 mousePos = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        //카메라 회전값
        Vector3 cameraRot = cameraAnchor.transform.localEulerAngles;

        //변경될 마우스 엥커 회전값의 X값
        float x = cameraRot.x - mousePos.y;

        //카메라 상하 제한
        if (x > cameraAngle && x < 180)
            x = cameraAngle;
        else if (x < 360 - cameraAngle && x > 180)
            x = 360 - cameraAngle;

        //카메라 위치값 적용
        cameraAnchor.transform.localEulerAngles = new Vector3(x, cameraRot.y + mousePos.x, cameraRot.z);
        directionObject.transform.localEulerAngles = new Vector3(x, cameraRot.y + mousePos.x, cameraRot.z);

        //플레이어 오브젝트가 보고있는 앞쪽 백터
        playerObjectForwardVector = cameraAnchor.transform.position - directionObject.transform.position;




        //줌 되지 않은 기본 상태의 카메라라면 카메라가 벽을 넘지 않도록 카메라 가더를 사용한다.
        if(cameraMode == 0)
        CameraWallGuarder(0);

        
    }

    private void CameraWallGuarder(float rayDistance)
    {
        //RayCast를 사용해서 만약 카메라가 오브젝트를 뚫었다면 카메라를 오브젝트 바깥으로 가저옴

        if (rayDistance <= 0)
            rayDistance = Vector3.Distance(cameraAnchor.transform.position, directionObject.transform.position);

        Vector3 rayVector = mainCamera.transform.position - transform.position;
        RaycastHit cameraLinkRay;
        Physics.Raycast(transform.position, rayVector, out cameraLinkRay, rayDistance);
        Debug.DrawRay(transform.position, rayVector, Color.red);


        //충돌한거 있으면 충돌한 위치로 카메라를 가져옴 충돌하지 않았다면 원레 카메라 위치로 돌려둠
        if (cameraLinkRay.point == Vector3.zero) 
        {
            mainCamera.transform.localPosition = cameraVector[cameraMode];
            cameraWallDown = false;
            Debug.Log($"cameraWallDown\nray.point : {cameraLinkRay.point} / tag : 없음");
        }
        else if (cameraLinkRay.point != Vector3.zero && cameraLinkRay.collider.gameObject.tag != "Player")
        {
            mainCamera.transform.position = cameraLinkRay.point;
            cameraWallDown = true;
            Debug.Log($"cameraWallUp \nray.point : {cameraLinkRay.point} / name : {cameraLinkRay.collider.gameObject.name}");
        }
    }

    private void CameraMode()
    {
        if (Input.GetMouseButtonUp(1))
        {
            cameraMode = 0;
        }
        else if (Input.GetMouseButtonDown(1))
        {
            cameraMode = 1;
        }
    }
}
