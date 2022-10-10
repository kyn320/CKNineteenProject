using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Landmark
{
    public class LandmarkWorkState : LandmarkStateBase
    {

        public override void Enter()
        {
            isStay = true;
            enterEvent?.Invoke();
        }

        public override void Exit()
        {
            isStay = false;
            exitEvent?.Invoke();
        }

        public override void Update()
        {
            return;
        }
    }
}