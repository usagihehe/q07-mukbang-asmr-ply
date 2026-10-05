using Spine.Unity;
using UnityEngine;

namespace PLY33.Blindbox
{
    [RequireComponent(typeof(BlindboxStateMachine))]
    public abstract class BlindboxState : MonoBehaviour, IState
    {
        private BlindboxStateMachine _stateMachine;

        // Lazy: the machine's OnEnable may enter a state before that state's Awake has run.
        protected BlindboxStateMachine Machine =>
            _stateMachine != null ? _stateMachine : _stateMachine = GetComponent<BlindboxStateMachine>();

        protected SkeletonGraphic Model => Machine.Model;

        public abstract string StateName { get; }

        public virtual void OnEnter()
        {
        }

        public virtual void OnExit()
        {
        }

        public abstract string OnUpdate(float deltaTime);

        public virtual bool IsSuitable()
        {
            return true;
        }
    }
}
