using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

[RequireComponent(typeof(Image))]
public class UIBaseImage : MonoBehaviour
{
    [ShowInInspector]
    [ReadOnly]
    protected Image Image;

    [Button("¿ÀÅä Ä³½Ì")]
    public void AutoCaching() {
        Image = GetComponent<Image>();
    }

    private void Awake()
    {
        AutoCaching();
    }

    public void SetImage(Sprite sprite) { 
        Image.sprite = sprite;  
    }

    public void SetColor(Color color)
    {
        Image.color = color;
    }

}
