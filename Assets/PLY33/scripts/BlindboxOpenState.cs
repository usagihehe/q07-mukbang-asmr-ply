using DG.Tweening;
using UnityEngine;

namespace PLY33.Blindbox
{
    /// <summary>Drops a ball holding a random food, then waits until the player picks it.</summary>
    public class BlindboxOpenState : BlindboxState
    {
        private const string IdleAnim = "idle";

        [SerializeField] private float _timeMove = 0.5f;
        [SerializeField] private Ease _ease = Ease.OutBounce;

        public override string StateName => BlindboxStateMachine.Open;

        public override void OnEnter()
        {
            Model.AnimationState.SetAnimation(0, IdleAnim, true);
            BlindboxSlotShelf ball = Machine.Blindbox;
            ball.Init(Machine.GetRandomItem());
            Machine.ResetBlindboxPos();
            ball.BallTransform.DOAnchorPosY(Machine.RestPosY, _timeMove)
                .SetEase(_ease)
                .SetLink(ball.gameObject)
                .OnComplete(() => ball.CanPick = true);
        }

        public override string OnUpdate(float deltaTime)
        {
            return Machine.Blindbox.IsPicked ? BlindboxStateMachine.Idle : null;
        }

        public override void OnExit()
        {
            Machine.ResetBlindboxPos();
        }
    }
}
