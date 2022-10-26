using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitPauseable
{
    void HitPause(float playWaitTime, float lifeTime);
    IEnumerator CoHitPause(float playWaitTime, float lifeTime);


}
