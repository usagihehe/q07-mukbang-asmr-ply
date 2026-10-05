using UnityEngine;

namespace PLY33.Blindbox
{
    /// <summary>The machine winds up before dispensing.</summary>
    public class BlindboxActState : BlindboxState
    {
        private const string ActAnim = "act";

        [SerializeField] private float _delayToAct = 0.5f;

        private float _elapsedTime;

        public override string StateName => BlindboxStateMachine.Act;

        public override void OnEnter()
        {
            _elapsedTime = 0f;
            Model.AnimationState.SetAnimation(0, ActAnim, false);
        }

        public override string OnUpdate(float deltaTime)
        {
            _elapsedTime += deltaTime;
            return _elapsedTime >= _delayToAct ? BlindboxStateMachine.Gacha : null;
        }
    }
}
