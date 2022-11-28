using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnUIMoveController : MonoBehaviour
{
    /*
    1. RectTransformUtility.WorldToScreenPoint()는 Vector2(0,0)값부터 게임 해상도 크기를 기준으로 값을 줍니다. 따라서 해상도 값을 resolutionRatio에 저장합니다.
    2. 실제 Canvas값과 게임 해상도 크기는 같지 않을 확률이 큽니다. 따라서, 실제 켄버스값을 canvasSize에 저장합니다.
    */

    RectTransform rectTrs;


    private Vector3 centerPos;
    private Vector3 widePos;
    [SerializeField]
    private Vector3 canvasSize;


    private void Awake()
    {
        rectTrs = GetComponent<RectTransform>();

        if(canvasSize == Vector3.zero)
        {
            if (transform.root.gameObject.GetComponent<Canvas>())
            {
                canvasSize = new Vector3(1920, 1080, 0);
                /*
                canvasSize.x = transform.root.gameObject.GetComponent<RectTransform>().rect.width;
                canvasSize.y = transform.root.gameObject.GetComponent<RectTransform>().rect.height;
                */
            }
        }



        centerPos = new Vector3(canvasSize.x/2, canvasSize.y/2, 0);
        widePos = new Vector3(Screen.width, Screen.height, 0);
    }


    //LockOnUI의 위치값을 변화합니다. InputLockOnMove등의 이벤트에 넣어서 사용해주세요
    //(UI 위치값을 변경하는 변수가 따로 존재한다면 해당 변수는 삭제하고 대신 사용해주시면 감사하겠습니다.)
    public void LockOnMove(Vector3 moveVector)
    {
        
        if (moveVector == Vector3.zero) 
           GetComponent<RectTransform>().anchoredPosition = centerPos;
        else
        {

            Vector3 worldPos = RectTransformUtility.WorldToScreenPoint(Camera.main, moveVector);
            rectTrs.anchoredPosition = new Vector3(worldPos.x * (canvasSize.x / widePos.x), worldPos.y * (canvasSize.y / widePos.y), 0);
        }
        
    }
}
