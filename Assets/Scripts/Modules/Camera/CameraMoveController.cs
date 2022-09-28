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

    //�ٸ� ��ũ��Ʈ�� �����Ҷ� ���� ī�޶� �ٶ󺸴� ����
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
        

        //���콺 ����Ʈ ��ġ
        Vector2 mousePos = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        //ī�޶� ȸ����
        Vector3 cameraRot = cameraAnchor.transform.localEulerAngles;

        //����� ���콺 ��Ŀ ȸ������ X��
        float x = cameraRot.x - mousePos.y;

        //ī�޶� ���� ����
        if (x > cameraAngle && x < 180)
            x = cameraAngle;
        else if (x < 360 - cameraAngle && x > 180)
            x = 360 - cameraAngle;

        //ī�޶� ��ġ�� ����
        cameraAnchor.transform.localEulerAngles = new Vector3(x, cameraRot.y + mousePos.x, cameraRot.z);
        directionObject.transform.localEulerAngles = new Vector3(x, cameraRot.y + mousePos.x, cameraRot.z);

        //�÷��̾� ������Ʈ�� �����ִ� ���� ����
        playerObjectForwardVector = cameraAnchor.transform.position - directionObject.transform.position;




        //�� ���� ���� �⺻ ������ ī�޶��� ī�޶� ���� ���� �ʵ��� ī�޶� ������ ����Ѵ�.
        if(cameraMode == 0)
        CameraWallGuarder(0);

        
    }

    private void CameraWallGuarder(float rayDistance)
    {
        //RayCast�� ����ؼ� ���� ī�޶� ������Ʈ�� �վ��ٸ� ī�޶� ������Ʈ �ٱ����� ������

        if (rayDistance <= 0)
            rayDistance = Vector3.Distance(cameraAnchor.transform.position, directionObject.transform.position);

        Vector3 rayVector = mainCamera.transform.position - transform.position;
        RaycastHit cameraLinkRay;
        Physics.Raycast(transform.position, rayVector, out cameraLinkRay, rayDistance);
        Debug.DrawRay(transform.position, rayVector, Color.red);


        //�浹�Ѱ� ������ �浹�� ��ġ�� ī�޶� ������ �浹���� �ʾҴٸ� ���� ī�޶� ��ġ�� ������
        if (cameraLinkRay.point == Vector3.zero) 
        {
            mainCamera.transform.localPosition = cameraVector[cameraMode];
            cameraWallDown = false;
            //Debug.Log($"cameraWallDown\nray.point : {cameraLinkRay.point} / tag : ����");
        }
        else if (cameraLinkRay.point != Vector3.zero && cameraLinkRay.collider.gameObject.tag != "Player")
        {
            mainCamera.transform.position = cameraLinkRay.point;
            cameraWallDown = true;
            //Debug.Log($"cameraWallUp \nray.point : {cameraLinkRay.point} / name : {cameraLinkRay.collider.gameObject.name}");
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
