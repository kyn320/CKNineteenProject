using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnUIMoveController : MonoBehaviour
{
    RectTransform rectTrs;

    private void Awake()
    {
        rectTrs = GetComponent<RectTransform>();
    }

    //LockOnUI의 위치값을 변화합니다. InputLockOnMove등의 이벤트에 넣어서 사용해주세요
    //(UI 위치값을 변경하는 변수가 따로 존재한다면 해당 변수는 삭제하고 대신 사용해주시면 감사하겠습니다.)
    public void LockOnMove(Vector3 moveVector)
    {
        if (moveVector == Vector3.zero)
            GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        else
            transform.position = Camera.main.WorldToScreenPoint(moveVector);
    }
}
