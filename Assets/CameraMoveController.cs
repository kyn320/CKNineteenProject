using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveController : MonoBehaviour
{

    public GameObject mainCamera;
    private GameObject cameraAnchor;
    private Vector3 cameraDefaultVector;
    public float cameraAngle = 80;

    //�� �ȵǴ��� Ȯ�ο�
    public Vector3 rayPoint;
    public Vector3 rayVector;
    public Vector3 anchorVector;
    public float di;


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
        //���콺�� ���� ī�޶� ������
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


        //RayCast�� ����ؼ� ���� ī�޶� ������Ʈ�� �վ��ٸ� ī�޶� ������Ʈ �ٱ����� ������

        rayVector = mainCamera.transform.position - cameraAnchor.transform.position;
        anchorVector = cameraAnchor.transform.position;
        di = Vector3.Distance(cameraAnchor.transform.position, rayVector);


        Debug.DrawRay(cameraAnchor.transform.position, rayVector, Color.red);
        RaycastHit ray;
        Physics.Raycast(cameraAnchor.transform.position, rayVector, out ray, Vector3.Distance(cameraAnchor.transform.position, mainCamera.transform.position));


        //�浹�Ѱ� ������ �浹�� ��ġ�� ī�޶� ������ �浹���� �ʾҴٸ� ���� ī�޶� ��ġ�� ������
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
