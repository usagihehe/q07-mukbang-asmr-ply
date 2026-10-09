using Spine.Unity;
using UnityEngine;

namespace PLY33.Blindbox
{
    [RequireComponent(typeof(BlindboxStateMachine))]
    public abstract class BlindboxState : MonoBehaviour, IState
    {
        [SerializeField]
        protected string _StateName;

        [SerializeField]
        protected string[] _NextStates;

        [SerializeField]
        protected string[] _CrossStates;

        protected SkeletonGraphic _Model;

        protected BlindboxStateMachine _StateMachine;

        public virtual string StateName
        {
            get => _StateName;
            protected set => _StateName = value;
        }

        public virtual string[] NextStates
        {
            get => _NextStates;
            protected set => _NextStates = value;
        }

        public virtual string[] CrossStates
        {
            get => _CrossStates;
            protected set => _CrossStates = value;
        }

        protected virtual void Awake()
        {
            _StateMachine = GetComponent<BlindboxStateMachine>();
            _Model = _StateMachine.Model;
        }

        public virtual void OnEnter()
        {
        }

        public virtual void OnExit()
        {
        }

        public abstract string OnUpdate(float deltaTime);

        public abstract bool IsSuitable();
    }
}
