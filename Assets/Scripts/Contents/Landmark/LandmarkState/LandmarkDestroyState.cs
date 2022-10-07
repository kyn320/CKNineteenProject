using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Landmark
{
    public class LandmarkDestroyState : LandmarkStateBase
    {
        public override void Enter()
        {
            //TODO :: ���帶ũ �ı� �� ������� ����
            //�׽�Ʈ ���� �Ⱓ���� Active�� ����
            controller.Destroy();
            enterEvent?.Invoke();
        }

        public override void Exit()
        {

        }

        public override void Update()
        {
            return;
        }
    }
}