using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BuffTestPlayer))]
public class BuffSystem : MonoBehaviour
{
    // BuffSystem 에서는 Buff, DeBuff 2가지를 관리한다.

}
/*
 * 버프는 Create Buff와 Create Debuff로 2가지가 나뉘어져 있다.
 * 각자 1개의 Buffer를 보유하고 있으며, 
 * 버퍼는 Timer를 보유하고 있다. 버퍼가 가져야 하는 데이터로는
 * 능력치의 원형과 Effect Time, Variance Count가 있다.
 */