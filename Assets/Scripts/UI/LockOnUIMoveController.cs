using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnUIMoveController : MonoBehaviour
{
    /*
    1. RectTransformUtility.WorldToScreenPoint()�� Vector2(0,0)������ ���� �ػ� ũ�⸦ �������� ���� �ݴϴ�. ���� �ػ� ���� resolutionRatio�� �����մϴ�.
    2. ���� Canvas���� ���� �ػ� ũ��� ���� ���� Ȯ���� Ů�ϴ�. ����, ���� �˹������� canvasSize�� �����մϴ�.
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


    //LockOnUI�� ��ġ���� ��ȭ�մϴ�. InputLockOnMove���� �̺�Ʈ�� �־ ������ּ���
    //(UI ��ġ���� �����ϴ� ������ ���� �����Ѵٸ� �ش� ������ �����ϰ� ��� ������ֽø� �����ϰڽ��ϴ�.)
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
