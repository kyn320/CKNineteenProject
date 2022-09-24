using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveController : MonoBehaviour
{

    private GameObject mainCamera;
    private GameObject cameraAnchor;
    public Vector3[] cameraVector;
    public int cameraMode;
    public float cameraAngle = 80;
    //�ٸ� ��ũ��Ʈ�� �����Ҷ� ���� ī�޶� �ٶ󺸴� ����
    public Vector3 cameraFrontVector;
    //���� ��ġ
    public Vector3 realVector;


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
            mainCamera = GameObject.Find("Main Camera");
            if (mainCamera == null)
            {
                Debug.LogError($"{this.gameObject.name} has no Camera...");
            }
        }

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
            //����� x�� ����
            cameraFrontVector = ((new Vector3(mainCamera.transform.position.x - cameraVector[cameraMode].x, mainCamera.transform.position.y, mainCamera.transform.position.z)
                - cameraAnchor.transform.position)* -1).normalized;

            Debug.DrawRay(cameraAnchor.transform.position, cameraFrontVector, Color.red);

            if (mainCamera.transform.localPosition != cameraVector[cameraMode])
            {
                mainCamera.transform.localPosition = cameraVector[cameraMode];
            }
        }

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

        if (cameraMode == 0)
        {
            //RayCast�� ����ؼ� ���� ī�޶� ������Ʈ�� �վ��ٸ� ī�޶� ������Ʈ �ٱ����� ������
            Vector3 rayVector = mainCamera.transform.position - cameraAnchor.transform.position;
            Debug.DrawRay(cameraAnchor.transform.position, rayVector, Color.red);
            RaycastHit ray;
            Physics.Raycast(cameraAnchor.transform.position, rayVector, out ray, Vector3.Distance(cameraAnchor.transform.position, mainCamera.transform.position));

            //�ٸ� ��ũ��Ʈ�� �����Ҷ� ���� ī�޶� �ٶ󺸴� ����
            cameraFrontVector = (rayVector.normalized) * -1;

            //�浹�Ѱ� ������ �浹�� ��ġ�� ī�޶� ������ �浹���� �ʾҴٸ� ���� ī�޶� ��ġ�� ������
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
