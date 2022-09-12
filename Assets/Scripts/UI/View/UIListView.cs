using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIListView<T> : MonoBehaviour where T : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform contentParent;

    public GameObject contentPrefab;

    public List<T> contentList = new List<T>();
    public Queue<T> unUsedContentQueue = new Queue<T>();

    public T AddContent()
    {
        T content;

        if (unUsedContentQueue.Count > 0)
        {
            content = unUsedContentQueue.Dequeue();
            content.gameObject.SetActive(true);
        }
        else
        {
            content = Instantiate(contentPrefab, contentParent).GetComponent<T>();
        }

        contentList.Add(content);
        return content;
    }

    public void RemoveAll() {
        foreach (var content in contentList) {
            content.gameObject.SetActive(false);
            unUsedContentQueue.Enqueue(content);
        }

        contentList.Clear();
    }

}
