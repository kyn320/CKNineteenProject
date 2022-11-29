using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Landmark
{
    public class LandmarkWorkState : LandmarkStateBase
    {
        [SerializeField]
        private float uiDeleteTime = .0f;

        public override void Enter()
        {
            isStay = true;
            enterEvent?.Invoke();

            Destroy(controller.uiLandmarkView.gameObject, uiDeleteTime);
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