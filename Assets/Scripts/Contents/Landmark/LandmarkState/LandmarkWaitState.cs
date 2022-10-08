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
            //TODO :: 미리 상호작용 가이드UI 생성하기
            interactionUIObject = UIController.Instance.CreateWorldUI(interactionUIPrefab);
            interactionUIObject.GetComponent<UITargetFollower>().SetTarget(interactionTargetObject);
            isStay = true;
            enterEvent?.Invoke();
        }

        public override void Exit()
        {
            //TODO :: 상호작용 가이드 UI 표시되고 있으면, 삭제하기
            if (interactionUIObject != null)
            {
                Destroy(interactionUIObject);
            }

            isStay = false;
            exitEvent?.Invoke();
        }

        public override void Update()
        {
            //TODO :: 상호작용 가이드 UI 범위내에 들어온 경우 표시 / 숨기기
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