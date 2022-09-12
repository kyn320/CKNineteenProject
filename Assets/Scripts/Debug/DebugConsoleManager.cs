using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugConsoleManager : Singleton<DebugConsoleManager>
{
    public GameObject consoleObject;

    private ScrollRect consoleScrollRect;
    private TextMeshProUGUI log;

    private float deltaTime = .0f;

    private Vector2 canvasSize = Vector2.zero;

    public override void Awake()
    {
        if (consoleObject == null)
        {
            Debug.LogError("Console Object is not Found");
            Debug.Break();
        }

        canvasSize = GameObject.Find("Canvas").GetComponent<RectTransform>().sizeDelta;
        GetComponent<RectTransform>().sizeDelta = canvasSize;

        consoleScrollRect = consoleObject.GetComponentInChildren<ScrollRect>();
        log = consoleObject.GetComponentInChildren<TextMeshProUGUI>();
        //log.text = "";
    }
    public void AddCustomDebugLog(string msg)
    {
        log.text += (msg + "\n");
        SetBottomView();
    }

    private void SetBottomView()
    {
        consoleScrollRect.verticalNormalizedPosition = 0;
    }

    private void Update()
    {
        deltaTime = (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    public void OnGUI()
    {
        float fps = 1.0f / deltaTime;
        float msec = deltaTime * 1000.0f;
        string text = string.Format("{0:0.} fps)",fps);

        GUI.Label(new Rect(canvasSize.x - 100f, 10f, 100f, 100f), text);
    }
}
