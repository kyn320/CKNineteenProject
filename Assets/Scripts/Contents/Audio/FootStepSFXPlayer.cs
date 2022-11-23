using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepSFXPlayer : RandomSFXPlayer
{
    [SerializeField]
    private float stepDistance = 0.5f;

    [SerializeField]
    private Vector3 prevPlayPosition;


    protected override void Update()
    {
        var distance = (transform.position - prevPlayPosition).sqrMagnitude;
        if (distance >= stepDistance * stepDistance)
        {
            Play();
            prevPlayPosition = transform.position;
        }

        base.Update();
    }


}
