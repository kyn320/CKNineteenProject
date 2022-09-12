using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UIData
{
    public UIType uiType;
    public string viewName = "";
    public virtual string prefabPath => $"UI/UI{viewName}";
}
