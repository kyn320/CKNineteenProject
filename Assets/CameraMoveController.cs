using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveController : MonoBehaviour
{

    private GameObject mainCamera;
    private GameObject cameraAnchor;
    private Vector3 cameraDefaultVector;
    public float cameraAngle = 80;

    //왜 안되는지 확인용
    public Vector3 rayPoint;
    public Vector3 rayVector;
    public Vector3 AnchorVector;


    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        if(cameraAnchor == null)
        { 
            cameraAnchor = this.gameObject; 
        }

        if(mainCamera == null)
        {
            mainCamera = this.transform.Find("Main Camera").gameObject;
            if (mainCamera == null)
            {
                Debug.LogError($"{this.gameObject.name} has no Camera...");
            }
        }
        cameraDefaultVector = mainCamera.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        CameraMove();
    }

    private void CameraMove()
    {
        //마우스에 따라서 카메라가 움직임
        Vector2 mousePos = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 cameraRot = cameraAnchor.transform.localEulerAngles;


        float x = cameraRot.x - mousePos.y;

        if (x > cameraAngle && x < 180)
        {
            Debug.Log($"1cameraAngle : {x}");
            x = cameraAngle;
        }
        else if (x < 360 - cameraAngle && x > 180)
        {
            Debug.Log($"2cameraAngle : {x}");
            x = 360 - cameraAngle;
        }

        cameraAnchor.transform.localEulerAngles = new Vector3(x, cameraRot.y + mousePos.x, cameraRot.z);


        //RayCast를 사용해서 만약 카메라가 오브젝트를 뚫었다면 카메라를 오브젝트 바깥으로 가저옴

        rayVector = mainCamera.transform.position;
        AnchorVector = cameraAnchor.transform.position;

        //엥커y값 변경시 따라서 바뀜
        if (cameraAnchor.transform.localPosition.y > 0)
            rayVector.y += cameraAnchor.transform.localPosition.y * -1;

        Debug.DrawRay(cameraAnchor.transform.position, rayVector, Color.red);
        RaycastHit ray;
        Physics.Raycast(cameraAnchor.transform.position, rayVector, out ray, Vector3.Distance(transform.position, cameraDefaultVector));

        //충돌한거 있으면 충돌한 위치로 카메라를 가져옴 충돌하지 않았다면 원레 카메라 위치로 돌려둠
        if (ray.point != Vector3.zero)
        {
            rayPoint = ray.point;
            mainCamera.transform.position = ray.point;
        }
        else
        {
            mainCamera.transform.localPosition = cameraDefaultVector;
        }
    }
}
