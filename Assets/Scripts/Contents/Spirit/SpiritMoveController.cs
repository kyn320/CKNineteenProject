using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritMoveController : MonoBehaviour
{
    //������ ��ǥ ����
    [SerializeField]
    private Transform targetTransform;

    //������ ������ ���ΰ��� ����
    public bool isMoveable = true;

    //������ �÷��̾� ���󰡴� �ӵ�
    public float dampingSpeed = 10;

    private void Awake()
    {
        transform.position = targetTransform.position;
        isMoveable = true;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!isMoveable)
            return;

        Move();
    }

    void Move()
    {
        transform.position = Vector3.Lerp(transform.position, targetTransform.position, dampingSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetTransform.rotation, dampingSpeed * Time.deltaTime);
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void SetRotation(Quaternion rotation)
    {
        transform.rotation = rotation;
    }

}
