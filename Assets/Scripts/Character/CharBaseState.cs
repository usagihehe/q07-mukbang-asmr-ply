using UnityEngine;

namespace Usaki
{
    public abstract class CharBaseState : MonoBehaviour
    {
        [SerializeField]
        protected string _stateName;

        [SerializeField]
        protected string[] _nextStates;

        protected CharacterCtrl _character;

        protected StateMachine _stateMachine;

        public string StateName => _stateName;

        public string[] NextStates => _nextStates;

        protected virtual void Awake()
        {
            _character = GetComponent<CharacterCtrl>();
            _stateMachine = GetComponent<StateMachine>();
        }

        public abstract void EnterState();

        public abstract string Execute(float dt);

        public abstract void ExitState();

        public abstract bool IsSuitable();
    }
}
