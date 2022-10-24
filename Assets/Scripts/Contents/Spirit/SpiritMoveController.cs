using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritMoveController : MonoBehaviour
{
    //������ ��ǥ ����
    [SerializeField]
    private Transform targetTransform;

    [SerializeField]
    private List<string> dontWallOutTagList;

    //������ ������ ���ΰ��� ����
    public bool isMoveable = true;

    //������ �÷��̾� ���󰡴� �ӵ�
    public float dampingSpeed = 10;

    //������ �����̴� ����
    public float spiritRepeatRange = 0.5f;

    //������ �����̴� ����
    public float spiritRepeatTime = 0.35f;
    float spiritRepeatTimer = 0f;
    Vector3 spiritRepeatVector = new Vector3(0, 0, 0);

    public LayerMask spiritDontHit;

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

        DontWallOutMove();
    }

    void Move(Vector3 targetPos)
    {
        spiritRepeatTimer += Time.fixedDeltaTime;

        if (spiritRepeatTimer > spiritRepeatTime)
        {
            spiritRepeatVector.x = Mathf.Lerp(0f, Random.Range(spiritRepeatRange * -1, spiritRepeatRange), spiritRepeatTimer);
            spiritRepeatVector.y = Mathf.Lerp(0f, Random.Range(spiritRepeatRange * -1, spiritRepeatRange), spiritRepeatTimer);
            spiritRepeatVector.z = Mathf.Lerp(0f, Random.Range(spiritRepeatRange * -1, spiritRepeatRange), spiritRepeatTimer);

            spiritRepeatTimer = 0;
        }

        transform.position = Vector3.Lerp(transform.position, targetPos + spiritRepeatVector, dampingSpeed * Time.fixedDeltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetTransform.rotation, dampingSpeed * Time.fixedDeltaTime);
    }

    void DontWallOutMove()
    {
        Vector3 parentPos = targetTransform.gameObject.transform.parent.transform.position;
        parentPos += new Vector3(0, targetTransform.localPosition.y, 0);

        float distance = Vector3.Distance(parentPos, targetTransform.transform.position);
        Vector3 direction = (targetTransform.position - parentPos).normalized;


        RaycastHit wallChacker;
        Physics.Raycast(parentPos, direction, out wallChacker, distance, spiritDontHit);
        Debug.DrawRay(parentPos, targetTransform.position - parentPos, Color.green);

        if (wallChacker.collider && dontWallOutTagList.Contains(wallChacker.collider.tag))
            Move(wallChacker.point - (direction * 0.3f));
        else
            Move(targetTransform.position);
    }
    public void SetRotation(Quaternion rotation)
    {
        transform.rotation = rotation;
    }
}