using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITargetFollower : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private Vector3 targetOffset;

    RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            Camera cam = Camera.main;
            var targetPos = RectTransformUtility.WorldToScreenPoint(Camera.main, target.position + targetOffset);

            rectTransform.anchoredPosition = targetPos;
        }
    }


}
