using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandmarkDestroyState : LandmarkStateBase
{
    public override void Action()
    {
        Destroy(gameObject);
        Destroy(manager.objHpBar);
    }
}
