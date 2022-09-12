using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIController : Singleton<UIController>
{
    public Camera uiCamera;
    public Canvas rootCanvas;

    public RectTransform worldGroup;
    public RectTransform viewGroup;
    public RectTransform popupGroup;

    public List<UIBaseView> viewList;
    public List<UIBasePopup> popupList;

    public RectTransform backgroundDimmed;

    public UIBaseView OpenView(UIBaseView view)
    {
        viewList.Add(view);

        view.Init(null);
        view.Open();

        return view;
    }

    public UIBaseView OpenView(UIBaseView view, UIData uiData)
    {
        viewList.Add(view);

        view.Init(uiData);
        view.Open();

        return view;
    }

    public UIBaseView OpenPopup(string popupName)
    {
        var popupPrefab = Resources.Load<GameObject>($"UI/UI{popupName}Popup");
        var popupObject = Instantiate(popupPrefab, popupGroup);
        var view = popupObject.GetComponent<UIBaseView>();
        var popupView = popupObject.GetComponent<UIBasePopup>();
        popupList.Add(popupView);

        if (popupView.useBackground)
        {
            backgroundDimmed.gameObject.SetActive(true);
        }

        backgroundDimmed.SetSiblingIndex(popupList.Count - 1);

        view.Init(null);
        view.Open();

        return view;
    }

    public UIBaseView OpenPopup(UIPopupData popupData)
    {
        var popupPrefab = Resources.Load<GameObject>(popupData.prefabPath);
        var popupObject = Instantiate(popupPrefab, popupGroup);
        var view = popupObject.GetComponent<UIBaseView>();
        var popupView = popupObject.GetComponent<UIBasePopup>();
        popupList.Add(popupView);

        if (popupView.useBackground)
        {
            backgroundDimmed.gameObject.SetActive(true);
        }

        backgroundDimmed.SetSiblingIndex(popupList.Count - 1);

        view.Init(popupData);
        view.Open();

        return view;
    }

    public void CloseView(UIBaseView view)
    {
        view.Close();
        viewList.Remove(view);
    }

    public void ClosePopup(UIBasePopup popup)
    {
        popupList.Remove(popup);

        if (backgroundDimmed.gameObject.activeSelf && popupList.Count > 0)
        {
            backgroundDimmed.gameObject.SetActive(true);
            backgroundDimmed.SetSiblingIndex(popupList.Count - 1);
        }
        else
        {
            backgroundDimmed.gameObject.SetActive(false);
        }

        popup.GetComponent<UIBaseView>().Close();
    }

    public void ClosePopup(string popupName)
    {
        var view = popupList.Find(item => item.viewName.Equals(popupName));

        popupList.Remove(view);

        if (backgroundDimmed.gameObject.activeSelf && popupList.Count > 0)
        {
            backgroundDimmed.gameObject.SetActive(true);
            backgroundDimmed.SetSiblingIndex(popupList.Count - 1);
        }
        else
        {
            backgroundDimmed.gameObject.SetActive(false);
        }

        view.Close();
    }

    public void ClosePopup(UIPopupData popupData)
    {
        var view = popupList.Find(item => item.viewName.Equals(popupData.viewName));

        popupList.Remove(view);

        if (backgroundDimmed.gameObject.activeSelf && popupList.Count > 0)
        {
            backgroundDimmed.gameObject.SetActive(true);
            backgroundDimmed.SetSiblingIndex(popupList.Count - 1);
        }
        else
        {
            backgroundDimmed.gameObject.SetActive(false);
        }

        view.Close();
    }

    public GameObject CreateWorldUI(GameObject uiPrefab)
    {
        return Instantiate(uiPrefab, worldGroup.transform);
    }

}
