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
            //���� üũ

            //�κ��丮 ����
            UIController.Instance.OpenPopup(new UIInventoryPopupData()
            {

            });
        }
    }


}
