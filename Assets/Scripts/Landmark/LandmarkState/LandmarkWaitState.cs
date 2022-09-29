using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandmarkWaitState : LandmarkStateBase
{
    public override void Action()
    {
        if (manager.objInteractionText == null)
        {
            Debug.Log("Create Text");
            CreateInteractionText();
        }
    }

    private void Update()
    {
        CreatePlayerSearchCircle(manager.callBoxRadius);

        if (manager.objInteractionText != null)
        {
            SetInteractionTextPos(CalcInteractionTextPos());
        }
    }

    private void CreateInteractionText()
    {
        manager.objInteractionText = Instantiate(manager.prefabsInteractionText);
        manager.objInteractionText.gameObject.SetActive(false);

        manager.objInteractionText.transform.SetParent(manager.canvasLandmark.transform);
        manager.rectTransformInteractionText = manager.objInteractionText.GetComponent<RectTransform>();
    }

    private void CreatePlayerSearchCircle(float _radius)
    {
        manager.coll = Physics.OverlapSphere(manager.objCallBox.transform.position, _radius);
        manager.collCount = 0;

        for (var i = 0; i < manager.coll.Length; i++)
        {
            if (manager.coll[i].CompareTag("Player"))
            {
                OnInteractionText(true);
                manager.collCount--;
            }
            else
            {
                manager.collCount++;
            }

            if (manager.collCount >= manager.coll.Length)
                OnInteractionText(false);
        }
    }
    private void SetInteractionTextPos(Vector3 pos)
    {
        manager.rectTransformInteractionText.position = pos;
    }

    private Vector3 CalcInteractionTextPos()
    {
        var objPos = Camera.main.WorldToScreenPoint(manager.objCallBox.transform.position);
        objPos.y += manager.interactionTextOffset;

        return objPos;
    }

    private void OnInteractionText(bool trigger)
    {
        manager.objInteractionText.gameObject.SetActive(trigger);
    }
}
