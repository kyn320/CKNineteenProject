using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDamageText : MonoBehaviour
{
    private Camera mainCamera;
    private Camera uiCamera;


    [SerializeField]
    RectTransform rectTransform;

    private Vector3 hitPoint;
    [SerializeField]
    private Vector3 offset;

    private RectTransform worldRectGroup;
    private Vector2 canvasRectPoint;

    [SerializeField]
    private UIAmountText amountText;

    public void SetDamageAmount(Vector3 hitPoint, float damage)
    {
        mainCamera = Camera.main;
        uiCamera = UIController.Instance.uiCamera;
        worldRectGroup = UIController.Instance.worldGroup;

        this.hitPoint = hitPoint + offset;

        amountText.UpdateAmount(damage);
    }

    private void Update()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(worldRectGroup
             , mainCamera.WorldToScreenPoint(hitPoint)
             , uiCamera
             , out canvasRectPoint);

        rectTransform.anchoredPosition = canvasRectPoint;
    }

    public void AutoDestroy()
    {
        Destroy(gameObject);
    }

}
