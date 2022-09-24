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
    //다른 스크립트에 참고할때 사용될 카메라가 바라보는 방향
    public Vector3 cameraFrontVector;
    //실제 위치
    public Vector3 bulletVector;


    public Animator anim;

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
        directionObject.transform.localPosition = mainCamera.transform.localPosition ;

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
        if (cameraMode != 0)
        {
            Debug.DrawRay(cameraAnchor.transform.position, cameraFrontVector, Color.red);

            if (mainCamera.transform.localPosition != cameraVector[cameraMode])
            {
                mainCamera.transform.localPosition = cameraVector[cameraMode];
            }
        }

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

        cameraFrontVector = cameraAnchor.transform.position - directionObject.transform.position;


        //카메라 줌 혹은 대화 등의 시야 변경시 카메라가 이동하지 않도록 제어
        if (cameraMode == 0)
        {
            //RayCast를 사용해서 만약 카메라가 오브젝트를 뚫었다면 카메라를 오브젝트 바깥으로 가저옴
            Vector3 rayVector = mainCamera.transform.position - cameraAnchor.transform.position;
            Debug.DrawRay(cameraAnchor.transform.position, rayVector, Color.red);
            RaycastHit ray;
            Physics.Raycast(cameraAnchor.transform.position, rayVector, out ray, Vector3.Distance(cameraAnchor.transform.position, mainCamera.transform.position));

            //충돌한거 있으면 충돌한 위치로 카메라를 가져옴 충돌하지 않았다면 원레 카메라 위치로 돌려둠
            if (ray.point != Vector3.zero && ray.collider.gameObject.tag != "Player")
            {
                mainCamera.transform.position = ray.point;
            }
            else
            {
                mainCamera.transform.localPosition = cameraVector[cameraMode];
            }
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
