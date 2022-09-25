using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LoadingTipData", menuName = "System/LoadingTipData", order = 0)]
public class LoadingTipData : ScriptableObject
{
    [SerializeField]
    private List<string> contentList = new List<string>();
    public List<string> ContentList { get { return contentList; } }

    public string GetRandomData()
    {
        return contentList[Random.Range(0, contentList.Count)];
    }

    public void SetData(List<string> contentList)
    {
        this.contentList = contentList;
    }
}
