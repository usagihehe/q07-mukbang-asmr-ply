using UnityEngine;

namespace PLY33.Blindbox
{
    /// <summary>The machine shakes while the ball is being drawn.</summary>
    public class BlindboxGachaState : BlindboxState
    {
        private const string GachaAnim = "gacha";

        [SerializeField] private float _gachaTime = 1f;

        private float _elapsedTime;

        public override string StateName => BlindboxStateMachine.Gacha;

        public override void OnEnter()
        {
            _elapsedTime = 0f;
            Model.AnimationState.SetAnimation(0, GachaAnim, true);
        }

        public override string OnUpdate(float deltaTime)
        {
            _elapsedTime += deltaTime;
            return _elapsedTime >= _gachaTime ? BlindboxStateMachine.Open : null;
        }
    }
}
