using UnityEngine;

namespace Pet
{
    public class PetDragState : PetBaseState
    {
        private PetDragCtrl _drag;

        protected override void Awake()
        {
            base.Awake();
            _drag = GetComponent<PetDragCtrl>();
        }

        public override void EnterState()
        {
            if (_drag == null)
                _drag = GetComponent<PetDragCtrl>();

            _character?.PlayAnimByName(PetCtrl.TOUCH, true, null);
            _character.PlaySound(PetCtrl.TOUCH);

        }

        public override string Execute(float dt)
        {
            if (_drag != null && (_drag.IsDragging))
                return PetStateMachine.DragState;
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
