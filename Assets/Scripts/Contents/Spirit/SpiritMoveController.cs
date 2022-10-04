using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritMoveController : MonoBehaviour
{
    //정령의 목표 벡터
    [SerializeField]
    private Transform targetTransform;

    //정령이 움직일 것인가의 여부
    public bool isMoveable = true;

    //정령이 플레이어 따라가는 속도
    public float dampingSpeed = 10;

    private void Awake()
    {
        transform.position = targetTransform.position;
        isMoveable = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!isMoveable)
            return;

        Move();
    }

    void Move()
    {
        transform.position = Vector3.Lerp(transform.position, targetTransform.position, dampingSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, transform.rotation, dampingSpeed * Time.deltaTime);
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
