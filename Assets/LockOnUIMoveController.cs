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


    public void LockOnMove(Vector3 moveVector)
    {
        if (moveVector == Vector3.zero)
            GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        else
            transform.position = Camera.main.WorldToScreenPoint(moveVector);
    }
}
