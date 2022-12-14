using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace Landmark
{
    public abstract class LandmarkStateBase : MonoBehaviour
    {
        protected LandmarkController controller;
        [SerializeField]
        protected LandmarkStateType stateType;

        [ReadOnly]
        [ShowInInspector]
        protected bool isStay = false;

        public UnityEvent enterEvent;
        public UnityEvent exitEvent;

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