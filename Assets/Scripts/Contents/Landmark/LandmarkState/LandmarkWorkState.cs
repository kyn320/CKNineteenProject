using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Landmark
{
    public class LandmarkWorkState : LandmarkStateBase
    {
        [SerializeField]
        private GameObject shieldObject;

        public override void Enter()
        {
            //랜드마크 보호막 생성
            //보호막 Scale 애니메이션
            shieldObject.SetActive(true);
        }

        public override void Exit()
        {
            shieldObject.SetActive(false);
        }

        public override void Update()
        {
            return;
        }
    }
}