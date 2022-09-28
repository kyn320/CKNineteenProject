using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOpener : MonoBehaviour
{



    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) { 
            
        }


        if (Input.GetKeyDown(KeyCode.Tab))
        {
            //상태 체크

            //인벤토리 오픈
            UIController.Instance.OpenPopup(new UIInventoryPopupData()
            {

            });
        }
    }


}
