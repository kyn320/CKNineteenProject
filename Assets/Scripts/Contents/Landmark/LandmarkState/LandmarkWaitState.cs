using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Landmark
{
    public class LandmarkWaitState : LandmarkStateBase
    {
        [SerializeField]
        private Transform interactionTargetObject;

        [SerializeField]
        private GameObject interactionUIPrefab;

        private GameObject interactionUIObject;

        private Collider[] overlapColliders;
        [SerializeField]
        private Vector3 offset;
        [SerializeField]
        private float overlapRadius;

        [SerializeField]
        private LayerMask layerMask;

        public override void Enter()
        {
            //TODO :: �̸� ��ȣ�ۿ� ���̵�UI �����ϱ�
            interactionUIObject = UIController.Instance.CreateWorldUI(interactionUIPrefab);
            interactionUIObject.GetComponent<UITargetFollower>().SetTarget(interactionTargetObject);
            isStay = true;
            enterEvent?.Invoke();
        }

        public override void Exit()
        {
            //TODO :: ��ȣ�ۿ� ���̵� UI ǥ�õǰ� ������, �����ϱ�
            if (interactionUIObject != null)
            {
                Destroy(interactionUIObject);
            }

            isStay = false;
            exitEvent?.Invoke();
        }

        public override void Update()
        {
            //TODO :: ��ȣ�ۿ� ���̵� UI �������� ���� ��� ǥ�� / �����
            overlapColliders = Physics.OverlapSphere(transform.position + offset, overlapRadius, layerMask);
            ShowUI(overlapColliders.Length > 0);
        }

        public void ShowUI(bool isShow)
        {
            interactionUIObject?.SetActive(isShow);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position + offset, overlapRadius);
        }
    }
}