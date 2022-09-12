using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSDebugger : MonoBehaviour
{
    [Range(1, 100)]
    public int fontSize = 100;
    public Color fontColor = Color.white;

    private float deltaTime = 0f;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    private void OnGUI()
    {
        int width = Screen.width;
        int height = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, width, height * 0.02f);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = height * 2 / fontSize;
        style.normal.textColor = fontColor;

        float mSec = deltaTime * 1000f;
        float fps = 1f / deltaTime;
        string text = string.Format("{0:0.0}ms , {1:0.} FPS", mSec, fps);
        GUI.Label(rect, text, style);
    }

}
