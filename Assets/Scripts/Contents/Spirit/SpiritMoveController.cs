using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritMoveController : MonoBehaviour
{
    //정령의 목표 벡터
    [SerializeField]
    private Transform targetTransform;

    [SerializeField]
    private List<string> dontWallOutTagList;

    //정령이 움직일 것인가의 여부
    public bool isMoveable = true;

    //정령이 플레이어 따라가는 속도
    public float dampingSpeed = 10;

    //정령이 움직이는 범위
    public float spiritRepeatRange = 0.5f;

    //정령이 움직이는 범위
    public float spiritRepeatTime = 0.35f;
    float spiritRepeatTimer = 0f;
    Vector3 spiritRepeatVector = new Vector3(0, 0, 0);

    public LayerMask spiritDontHit;

    //정령 회전 중심 오브젝트
    [SerializeField]
    GameObject turnTargetObject;

    //회전 반지름
    [SerializeField]
    float turnRadius = 0.2f;

    //회전 할 시간
    [SerializeField]
    float turnTime = 0.75f;
    float turnTimer = 0;

    //회전 속도
    [SerializeField]
    float moveSpeed = 1000;

    SpiritMoveController spiritMoveController;

    private void Awake()
    {
        spiritMoveController = GetComponent<SpiritMoveController>();
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


    //정령이 회전하는 동작의 트리거입니다. AttackInput Event등의 이벤트에 넣어서 사용해주세요
    public void WeaponSpawnMove()
    {
        if (spiritMoveController)
            spiritMoveController.enabled = false;

        isMoveable = false;

        StartCoroutine(TurnMove());
    }

    public IEnumerator TurnMove()
    {
        float deg = 0;

        while (true)
        {
            if (turnTime > turnTimer && !isMoveable)
            {
                yield return new WaitForFixedUpdate();
                turnTimer += Time.deltaTime;
                deg += Time.deltaTime * moveSpeed;

                if (deg < 360)
                {
                    var rad = Mathf.Deg2Rad * (deg);
                    var x = turnRadius * Mathf.Sin(rad);
                    var y = turnRadius * Mathf.Cos(rad);
                    Debug.Log($"x : {x} / y : {y} \ndeg : {deg}");
                    transform.position = turnTargetObject.transform.position + new Vector3(x, y, 0);
                }
                else
                    deg = 0;
            }
            else
            {
                isMoveable = true;

                if (spiritMoveController)
                    spiritMoveController.enabled = true;

                turnTimer = 0;
                yield break;
            }
        }
    }

    public void SetMoveable(bool isSetMoveable)
    {
        isMoveable = isSetMoveable;
    }
}