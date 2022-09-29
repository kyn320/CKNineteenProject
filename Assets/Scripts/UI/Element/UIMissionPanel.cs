using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMissionPanel : UIListView<UIMissionSlot>
{

    public void AddMission() { 
        var content = AddContent();
        content.SetMission();
    }


}
