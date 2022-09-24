using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Landmark
{
    public class LandmarkSystem : MonoBehaviour
    {
        private Landmark_State states;

        private int hp = 0;

        // �� ������
        [SerializeField]
        private float shieldRadius = .0f;
        // HP �� Ȯ��
        [SerializeField]
        private int healPercent = 0;
        // HP �� �ð�
        [SerializeField]
        private float healTime = .0f;

        private void Awake()
        {
            SetState(Landmark_State.LANDMARK_WAIT);
        }


        #region State

        public void SetState(Landmark_State state)
        {
            states = state;

            Debug.Log("LandMark State : " + states);
            CallStateInitialize(states);
        }

        public Landmark_State GetState()
        {
            return states;
        }

        #endregion

        #region Initialize

        private void CallStateInitialize(Landmark_State state)
        {
            switch (state)
            {
                case Landmark_State.NONE:
                    break;
                case Landmark_State.LANDMARK_WAIT:
                    WaitInitialize();
                    break;
                case Landmark_State.LANDMARK_READY:
                    ReadyInitialize();
                    break;
                case Landmark_State.LANDMARK_WORK:
                    WorkInitialize();
                    break;
                case Landmark_State.LANDMARK_DESTROY:
                    DestoryInitialize();
                    break;
            }
        }

        private void WaitInitialize()
        {

        }

        private void ReadyInitialize()
        {

        }

        private void WorkInitialize()
        {

        }

        private void DestoryInitialize()
        {

        }
        #endregion

        #region Wait
        public void OnActive()
        {
            Active();
        }

        private void Active()
        {
            SetState(Landmark_State.LANDMARK_READY);
        }

        #endregion

        #region Ready

        private void OnWorkChanged()
        {
            if(states == Landmark_State.LANDMARK_WAIT)
            {
                
            }
        }
        #endregion

        #region Work

        private void OnDestoryChanged()
        {

        }
        #endregion

        #region Destory
        #endregion



    }
}
