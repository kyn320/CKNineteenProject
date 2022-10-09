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
            //���帶ũ ��ȣ�� ����
            //��ȣ�� Scale �ִϸ��̼�
            shieldObject.SetActive(true);
            isStay = true;
            enterEvent?.Invoke();
        }

        public override void Exit()
        {
            shieldObject.SetActive(false);
            isStay = false;
            exitEvent?.Invoke();
        }

        public override void Update()
        {
            return;
        }
    }
}