using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveController : MonoBehaviour
{
    [SerializeField]
    private GameObject mainCamera;
    [SerializeField]
    private GameObject cameraAnchor;
    [SerializeField]
    private GameObject directionObject;

    public Vector3[] cameraVector;
    public bool isZoom;
    public float cameraAngle = 80;

    //�ٸ� ��ũ��Ʈ�� �����Ҷ� ���� ī�޶� �ٶ󺸴� ����
    public Vector3 playerObjectForwardVector;

    private void Awake()
    {
        cameraAnchor = this.gameObject;

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
        if (x > cameraAngle && x < 180f)
            x = cameraAngle;
        else if (x < 360f - cameraAngle && x > 180f)
            x = 360f - cameraAngle;

        //ī�޶� ��ġ�� ����
        cameraAnchor.transform.localEulerAngles = new Vector3(x, cameraRot.y + mousePos.x, cameraRot.z);
        directionObject.transform.localEulerAngles = new Vector3(x, cameraRot.y + mousePos.x, cameraRot.z);

        //�÷��̾� ������Ʈ�� �����ִ� ���� ����
        playerObjectForwardVector = cameraAnchor.transform.position - directionObject.transform.position;

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
            mainCamera.transform.localPosition = cameraVector[1];
        }
        else if (Input.GetKeyUp(KeyCode.R))
        {
            isZoom = false;
            mainCamera.transform.localPosition = cameraVector[0];
        }
    }
}
