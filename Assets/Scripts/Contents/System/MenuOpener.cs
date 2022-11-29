using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class MenuOpener : MonoBehaviour
{
    private UIPausePopupData pausePopupData;
    private UIInventoryPopupData inventoryPopupData;

    [ShowInInspector]
    [ReadOnly]
    private PlayerBattleStateType battleStateType;

    private void Start()
    {
        WorldController.Instance.endGameEvent.AddListener(CloseAllPopup);
    }

    public void UpdateBattleType(PlayerBattleStateType battleStateType)
    {
        this.battleStateType = battleStateType;
    }

    public void CloseAllPopup()
    {
        if (pausePopupData != null)
        {
            UIController.Instance.ClosePopup(pausePopupData);
        }

        if (inventoryPopupData != null)
        {
            UIController.Instance.ClosePopup(inventoryPopupData);
        }
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
            else if(battleStateType == PlayerBattleStateType.Normal)
            {
                inventoryPopupData = new UIInventoryPopupData();
                var view = UIController.Instance.OpenPopup(inventoryPopupData);
                view.closeEvent.AddListener(() =>
                {
                    inventoryPopupData = null;
                });
            }
        }
    }


}
