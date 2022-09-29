using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallMonster : MonsterBase
{
    public GameObject objView = null;

    private void Awake()
    {
        if(monsterStatus == null )
            Debug.Log(monsterStatus.GetType() + "is Not Found");

        if(objView == null)
            Debug.Log(objView.GetType() + "is Not Found");

        monsterType = MonsterType.MONSTERTYPE_SMALL;
    }

    private void Update()
    {
        //if(GetHP() <= 0)
        //{
        //    SetLife(false);
        //}
    }
}
