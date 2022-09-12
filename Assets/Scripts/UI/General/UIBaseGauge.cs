using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIBaseGauge : MonoBehaviour
{
    public Image fillGaugeImage;

    public UnityEvent<float,float> updateDisplayEvent;
    public void UpdateGauge(float progress)
    {
        fillGaugeImage.fillAmount = progress;
    }

    public void UpdateGauge(float current, float max)
    {
        fillGaugeImage.fillAmount = current / max;
        updateDisplayEvent?.Invoke(current, max);   
    }

}
