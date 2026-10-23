using Spine;
using UnityEngine;

namespace Pet
{
    public class PetIdleState : PetBaseState
    {
        private TrackEntry entry;

        protected override void Awake()
        {
            base.Awake();
        }

        private void OnDisable()
        {
        }

        public override void EnterState()
        {
            if (_character == null || _stateMachine == null) base.Awake();
            entry = _character.PlayAnimByName(PetCtrl.IDLE, true);
        }

        public override string Execute(float dt)
        {
            if (_stateMachine.Held != null && _stateMachine.Distance() < _stateMachine.DistanceEating)
                return PetStateMachine.EatingState;
            return PetStateMachine.IdleState;
        }

        public override void ExitState()
        {
        }

        public override bool IsSuitable()
        {
            return true;
        }
    }
}
