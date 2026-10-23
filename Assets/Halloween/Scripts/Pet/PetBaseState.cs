using UnityEngine;

namespace Pet
{
    public abstract class PetBaseState : MonoBehaviour
    {
        [SerializeField]
        protected string _stateName;

        [SerializeField]
        protected string[] _nextStates;

        protected PetCtrl _character;

        protected PetStateMachine _stateMachine;

        public string StateName => _stateName;

        public string[] NextStates => _nextStates;

        protected virtual void Awake()
        {
            _character = GetComponent<PetCtrl>();
            _stateMachine = GetComponent<PetStateMachine>();
        }

        public abstract void EnterState();

        public abstract string Execute(float dt);

        public abstract void ExitState();

        public abstract bool IsSuitable();
    }
}
