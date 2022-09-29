using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOpener : MonoBehaviour
{
    private UIPausePopupData pausePopupData;
    private UIInventoryPopupData inventoryPopupData;

    private void Awake()
    {

    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && inventoryPopupData == null)
        {
            if (pausePopupData != null)
            {
                UIController.Instance.ClosePopup(pausePopupData);
                pausePopupData = null;
            }
            else
            {
                pausePopupData = new UIPausePopupData();
                UIController.Instance.OpenPopup(pausePopupData);
            }
        }


        if (Input.GetKeyDown(KeyCode.Tab) && pausePopupData == null)
        {
            if (inventoryPopupData != null)
            {
                UIController.Instance.ClosePopup(inventoryPopupData);
                inventoryPopupData = null;
            }
            else
            {
                inventoryPopupData = new UIInventoryPopupData();
                var view =  UIController.Instance.OpenPopup(inventoryPopupData);
                view.closeEvent.AddListener(() => { 
                    inventoryPopupData = null;
                });
            }
        }
    }


}
