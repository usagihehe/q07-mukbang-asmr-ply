using DG.Tweening;
using UnityEngine;

namespace PLY33.Blindbox
{
    /// <summary>The machine shakes once while the ball is being drawn.</summary>
    public class BlindboxGachaState : BlindboxState
    {
        [Tooltip("Extra wait after the gacha animation ends, before the ball drops.")]
        [SerializeField]
        private float _gachaTime;

        private bool _isAct;

        public override void OnEnter()
        {
            DOTween.Kill(this);
            _isAct = false;
            _Model.AnimationState.SetAnimation(0, _StateMachine.Gacha, false).Complete += _ =>
                DOVirtual.DelayedCall(_gachaTime, () => _isAct = true).SetId(this).SetLink(gameObject);
        }

        public override string OnUpdate(float deltaTime)
        {
            return _isAct ? _StateMachine.Open : null;
        }

        public override void OnExit()
        {
            DOTween.Kill(this);
        }

        public override bool IsSuitable()
        {
            return true;
        }
    }
}
