using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UICreaditView : MonoBehaviour
{
    [SerializeField]
    private string nextScene;

    private float currentShowTime = 0f;
    public float showTime = 5f;

    [SerializeField]
    private RectTransform creaditBound;

    public UnityEvent startCreaditEvent;
    public UnityEvent endCreaditEvent;

    private Vector3 startPoint;
    private Vector3 endPoint;

    private bool isUpdate = false;

    private void Start()
    {
        FadeController.Instance.SetActive(false);
        Canvas.ForceUpdateCanvases();

        startPoint = creaditBound.anchoredPosition;
        endPoint = startPoint + Vector3.up * creaditBound.rect.height * 1.5f;
        startCreaditEvent?.Invoke();
        currentShowTime = 0f;
        isUpdate = true;
    }


    private void Update()
    {
        if (!isUpdate)
            return;

        currentShowTime += Time.deltaTime;
        creaditBound.anchoredPosition = Vector3.Lerp(startPoint, endPoint, currentShowTime / showTime);

        if (currentShowTime >= showTime)
        {
            isUpdate = false;
            endCreaditEvent?.Invoke();
            FadeController.Instance.SetActive(true);
            SceneLoader.Instance.SwitchDirectScene(nextScene);
        }
    }


}
