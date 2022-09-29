using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandmarkWorkState : LandmarkStateBase
{
    public override void Action()
    {
        CreateField();
    }
    private void Update()
    {
        if (manager.objHpBar != null)
        {
            manager.SetHpBarPos(manager.CalcHpBarPos());
            manager.CalcHp();
        }
    }
    private void CreateField()
    {
        manager.objField.SetActive(true);

        StartCoroutine("ScaleField", manager.fieldRadius);
        PlayFieldEffect();

        manager.isOnField = true;
    }

    private IEnumerator ScaleField(float radius)
    {
        var fieldScale = manager.objField.transform.localScale;
        var diameter = radius * 2;

        for (var i = 0; i < diameter; i++)
        {
            manager.objField.transform.localScale = new Vector3(fieldScale.x + i, fieldScale.y + i, fieldScale.z + i);
            PlayFieldEffect();
            yield return new WaitForSeconds(0.5f);
        }

        StopCoroutine("ScaleField");
    }

    private void PlayFieldEffect()
    {
        Collider[] fieldColl = Physics.OverlapSphere(manager.objField.transform.localPosition, manager.fieldRadius);

        for (var i = 0; i < fieldColl.Length; i++)
        {
            if (fieldColl[i].name == "Field") continue;
            Debug.Log(fieldColl[i].name);
        }

    }
}
