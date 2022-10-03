using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Landmark
{
    public abstract class LandmarkStateBase : MonoBehaviour
    {
        protected LandmarkController controller;
        [SerializeField]
        protected LandmarkStateType stateType;

        protected virtual void Awake()
        {
            controller = GetComponent<LandmarkController>();
            controller.AddState(stateType, this);
        }

        public abstract void Enter();

        public abstract void Update();

        public abstract void Exit();

    }
}