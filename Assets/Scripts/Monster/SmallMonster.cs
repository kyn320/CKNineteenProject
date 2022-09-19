using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallMonster : MonsterBase
{
    private void Update()
    {
        if(GetHP() <= 0)
        {
            SetLife(false);
        }
    }
}
