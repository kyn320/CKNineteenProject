using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainAttackFinder : MonoBehaviour
{
    [SerializeField]
    Collider[] targets;

    [SerializeField]
    float monsterSearchLength = 5f;

    [SerializeField]
    LayerMask targetLayer;

    [SerializeField]
    string[] chainLog;
    [SerializeField]
    int chainCount = 0;

    [SerializeField]
    int chainLimit = 5;


    private void Start()
    {
        chainLog = new string[0];
    }

    public void ChainAttackStart(Collider hitCollision)
    {
        if (Mathf.Pow(2, hitCollision.gameObject.layer) != targetLayer)
            return;

        if (targets.Length == 0)
            targets = Physics.OverlapSphere(transform.position, monsterSearchLength, targetLayer);

        //������ ������Ʈ Log ����
        if (chainLog.Length == 0)
        {
            chainLog = new string[targets.Length];
            for (int i = 0; i < targets.Length; i++)
            {
                chainLog[i] = targets[i].transform.root.gameObject.name;
            }
        }

        //�ߺ� ������ Log������
        for(int i = 0; i < targets.Length && i < chainLimit; i++)
        {
            if (targets[i].name == hitCollision.gameObject.transform.root.name)
            {
                chainLog[i] = "attackedName";

                //���� �ݶ��̴��� ���� ������ ��� �ߺ� üũ�� ����
                for (int j = 0; j < chainLog.Length; j++)
                    if (chainLog[j] == targets[i].transform.root.gameObject.name)
                        chainLog[j] = "attackedName";
            }
        }

        //Ÿ���� ����
        for (int i = 0; i < targets.Length && i < chainLimit; i++)
        {
            if (targets[i] && chainCount < chainLimit)
            {
                if(targets[i].transform.root.name == chainLog[i])
                {
                    chainCount++;
                    Vector3 targetDistance = targets[i].transform.position - transform.position;
                    gameObject.GetComponent<ProjectileStraightMove>().SetDirection(targetDistance.normalized);



                    return;
                }
            }
            else if (chainCount >= chainLimit)
            {
                Destroy(this.gameObject);
            }
        }
    }
}


