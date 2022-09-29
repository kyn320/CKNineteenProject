using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMissionSlot : MonoBehaviour
{
    [SerializeField]
    private UIBaseImage stateIcon;

    [SerializeField]
    private UIBaseText missionText;

    public void SetMission() {

        missionText.SetText("Dummy Mission A");
    }


    public void UpdateState() { 
    
    }

}
