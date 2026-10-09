using DG.Tweening;
using UnityEngine;

namespace PLY33.Blindbox
{
    /// <summary>The machine winds up before dispensing.</summary>
    public class BlindboxActState : BlindboxState
    {
        [SerializeField]
        private float _delayToAct = 0.5f;

        private bool _canEnterState;

        private bool _isAct;

        protected override void Awake()
        {
            base.Awake();
            _StateMachine.CatcherBtn.onClick.AddListener(() => _canEnterState = !_StateMachine.IsBasketFull);
        }

        public override void OnEnter()
        {
            // A delay left over from a round cut short by disabling the panel must not end this one early.
            DOTween.Kill(this);
            _canEnterState = false;
            _isAct = false;
            _StateMachine.CatcherBtn.interactable = false;
            DOVirtual.DelayedCall(_delayToAct, () =>
                    _Model.AnimationState.SetAnimation(0, _StateMachine.Act, false).Complete += _ => OnEndDelay())
                .SetId(this)
                .SetLink(gameObject);
        }

        public override string OnUpdate(float deltaTime)
        {
            return _isAct ? _StateMachine.Gacha : null;
        }

        public override bool IsSuitable()
        {
            return _canEnterState;
        }

        private void OnEndDelay()
        {
            _isAct = true;
        }
    }
}
