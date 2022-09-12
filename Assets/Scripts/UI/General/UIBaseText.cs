using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class UIBaseText : MonoBehaviour
{
    [ShowInInspector]
    [ReadOnly]
    protected TextMeshProUGUI text;

    [SerializeField]
    protected string frontAdditionalText = "";
    [SerializeField]
    protected string backAdditionalText = "";
    [SerializeField]
    protected string viewFormat = "";

    [Button("¿ÀÅä Ä³½Ì")]
    public void AutoCaching()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Awake()
    {
        AutoCaching();
    }

    public void SetText(string context)
    {
        if (text == null)
            AutoCaching();

        text.text = $"{frontAdditionalText}{(string.IsNullOrEmpty(viewFormat) ? context : string.Format(viewFormat, context))}{backAdditionalText}";
    }

    public void ChangeColor(Color color)
    {
        text.color = color;
    }
}
