using Spine;
using UnityEngine;

namespace Usaki
{
    public class IdleState : CharBaseState
    {
        [SerializeField]
        private float _timeHungry = 5;
        private float _leanId;
        private TrackEntry entry;

        protected override void Awake()
        {
            base.Awake();
            _leanId = 0;
        }

        private void OnDisable()
        {
            _leanId = 0;
        }

        public override void EnterState()
        {
            if (_character == null || _stateMachine == null) base.Awake();
            _leanId = 0;
            entry = _character.PlayAnimByName(CharacterCtrl.IDLE, true);
        }

        public override string Execute(float dt)
        {
            if (_stateMachine.MukbangItem != null && _stateMachine.IsSelectItem)
            {
                if (_stateMachine.Distance() < _stateMachine.DistanceEating)
                {
                    return StateMachine.EatingState;
                }
            }
            if (_stateMachine.IsSelectItem)
            {
                return StateMachine.BeforeEatState;
            }
            _leanId += dt;
            if (_leanId >= _timeHungry)
            {
                //if (entry.Animation != null && entry.Animation.Name != CharacterCtrl.SAD)
                //{
                //    entry = _character.PlayAnimByName(CharacterCtrl.SAD, false, delegate
                //    {
                //        entry = _character.PlayAnimByName(CharacterCtrl.IDLE, true);
                //    });
                //}
                _leanId = 0;
            }
            return StateMachine.IdleState;
        }

        public override void ExitState()
        {
            _leanId = 0;
        }

        public override bool IsSuitable()
        {
            return true;
        }
    }
}
