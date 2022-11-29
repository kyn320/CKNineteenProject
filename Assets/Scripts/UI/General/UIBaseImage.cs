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

    [Button("���� ĳ��")]
    public void AutoCaching() {
        Image = GetComponent<Image>();
    }

    private void Awake()
    {
        AutoCaching();
    }

    public void SetImage(Sprite sprite)
    {
        AutoCaching();
        Image.sprite = sprite;  
    }

    public void SetColor(Color color)
    {
        AutoCaching();
        Image.color = color;
    }

}
