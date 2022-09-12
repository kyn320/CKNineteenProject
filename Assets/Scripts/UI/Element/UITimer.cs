using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UITimer : MonoBehaviour
{
    [SerializeField]
    protected TextMeshProUGUI timerText;

    private void Start()
    {
        //WorldController.Instance.updatePlayTime.AddListener(UpdateTimer);
    }

    public void UpdateTimer(float currentTime, float maxTime)
    {
        timerText.text = string.Format("{0:F0}", maxTime - currentTime);
    }

}
