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

    public T GetView<T>(string viewName) where T : UIBaseView
    {
        var view = viewList.Find(item => item.viewName.Equals(viewName));

        if (view == null)
        {
            return null;
        }

        return (T)view;
    }

    public UIBaseView OpenView(UIBaseView view)
    {
        return OpenView(view, null);
    }

    public UIBaseView OpenView(UIData uiData)
    {
        var viewPrefab = Resources.Load<GameObject>(uiData.prefabPath);
        var viewObject = Instantiate(viewPrefab, viewGroup);
        var view = viewObject.GetComponent<UIBaseView>();

        return OpenView(view, uiData);
    }
    public UIBaseView OpenView(string viewName)
    {
        var viewPrefab = Resources.Load<GameObject>($"UI/UI{viewName}");
        var viewObject = Instantiate(viewPrefab, viewGroup);
        var view = viewObject.GetComponent<UIBaseView>();

        return OpenView(view, null);
    }

    public UIBaseView OpenView(UIBaseView view, UIData uiData)
    {
        viewList.Add(view);

        view.Init(uiData);
        view.BeginOpen();
        return view;
    }

    public void CloseView(string viewName)
    {
        var view = viewList.Find(item => item.viewName.Equals(viewName));

        CloseView(view);
    }
    public void CloseView(UIData uiData)
    {
        var view = viewList.Find(item => item.viewName.Equals(uiData.viewName));

        CloseView(view);
    }

    public void CloseView(UIBaseView view)
    {
        viewList.Remove(view);
        view.BeginClose();
    }

    public T GetPopup<T>(string popupName) where T : UIBasePopup
    {
        var popup = popupList.Find(item => item.viewName.Equals(popupName));

        if (popup == null)
        {
            return null;
        }

        return (T)popup;
    }

    public UIBasePopup OpenPopup(string popupName)
    {
        var popupPrefab = Resources.Load<GameObject>($"UI/UI{popupName}Popup");
        var popupObject = Instantiate(popupPrefab, popupGroup);
        var popupView = popupObject.GetComponent<UIBasePopup>();

        return OpenPopup(popupView, null);
    }

    public UIBasePopup OpenPopup(UIPopupData popupData)
    {
        var popupPrefab = Resources.Load<GameObject>(popupData.prefabPath);
        var popupObject = Instantiate(popupPrefab, popupGroup);
        var popupView = popupObject.GetComponent<UIBasePopup>();

        return OpenPopup(popupView, popupData);
    }

    public UIBasePopup OpenPopup(UIBasePopup popup, UIPopupData popupData)
    {

        popupList.Add(popup);

        if (popup.useBackground)
        {
            backgroundDimmed.gameObject.SetActive(true);
        }

        backgroundDimmed.SetSiblingIndex(popupList.Count - 1);

        popup.Init(popupData);
        popup.BeginOpen();

        return popup;
    }

    public void ClosePopup(string popupName)
    {
        var popup = popupList.Find(item => item.viewName.Equals(popupName));
        ClosePopup(popup);
    }

    public void ClosePopup(UIPopupData popupData)
    {
        var popup = popupList.Find(item => item.viewName.Equals(popupData.viewName));
        ClosePopup(popup);
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

        popup.BeginClose();
    }

    public GameObject CreateWorldUI(GameObject uiPrefab)
    {
        return Instantiate(uiPrefab, worldGroup.transform);
    }

}
