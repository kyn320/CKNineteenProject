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
    int chainCount = 0;

    [SerializeField]
    int chainLimit = 5;


    private void Start()
    {
        chainLog = new string[0];
        Debug.Log($"{this.gameObject.name} 생성됨");
    }

    public void ChainAttackStart(Collision hitCollision)
    {
        if (Mathf.Pow(2, hitCollision.gameObject.layer) != targetLayer)
        {
            return;
        }

        if (targets.Length == 0)
            targets = Physics.OverlapSphere(transform.position, monsterSearchLength, targetLayer);


        if (chainLog.Length == 0)
        {
            chainLog = new string[targets.Length];
            for (int i = 0; i < targets.Length; i++)
                chainLog[i] = targets[i].transform.root.gameObject.name;
        }

        for (int i = 0; i < targets.Length && i < chainLimit; i++)
        {
            if (targets[i].transform.root.name == chainLog[i] && chainCount < chainLimit)
            {
                chainCount++;
                Vector3 targetDistance = targets[i].transform.position - transform.position;
                gameObject.GetComponent<ProjectileStraightMove>().SetDirection(targetDistance.normalized);
                
                //다중 콜라이더를 지닌 몬스터일 경우 중복 체크를 방지
                for (int j = 0; j < chainLog.Length; j++)
                    if (chainLog[j] == targets[i].transform.root.gameObject.name)
                        chainLog[j] = "attackedName";

                chainLog[i] = "attackedName";

                return;
            } 
            else if (chainCount >= chainLimit)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
