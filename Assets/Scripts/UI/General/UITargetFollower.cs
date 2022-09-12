using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITargetFollower : MonoBehaviour
{
    public Transform target;

    RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void FixedUpdate()
    {
        if (target != null) {

            //Other Solution
            /*            
             
            var point = Vector2.zero;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(UIController.Instance.viewGroup, endPosition, UIController.Instance.uiCamera, out point);

            holdGaugeRectTransfrom.anchoredPosition = point; 
             
             */

            Camera cam = Camera.main;
            var targetPos = RectTransformUtility.WorldToScreenPoint(Camera.main, target.position);

            rectTransform.anchoredPosition = targetPos; 
        }   
    }


}
