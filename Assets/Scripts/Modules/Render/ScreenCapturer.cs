using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ScreenCapturer : MonoBehaviour
{
    [Button("ĸó")]
    public void Capture()
    {
        ScreenCapture.CaptureScreenshot($"HyperRace_{System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")}.png");
    }

}
