using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace PLY33.Blindbox
{
    /// <summary>Drops a ball holding a random food, then waits until the player picks it.</summary>
    public class BlindboxOpenState : BlindboxState
    {
        [SerializeField]
        private float _timveMove = 0.5f;

        [SerializeField]
        private Ease _ease = Ease.OutBounce;

        [SerializeField]
        private Mask _mask;

        private bool _isAct;

        public override void OnEnter()
        {
            _isAct = false;
            // No animation change: the one-shot gacha holds its last frame, keeping the gate open while the ball drops.
            BlindboxSlotShelf ball = _StateMachine.Blindbox;
            // Activate first: SkeletonGraphic builds its skeleton in Awake, which Init needs.
            ball.gameObject.SetActive(true);
            ball.Init(_StateMachine.DrawNextItem());
            // From(): the ball's place in the scene is where it lands.
            ((RectTransform)ball.transform).DOAnchorPosY(_StateMachine.StartPosY, _timveMove)
                .From()
                .SetEase(_ease)
                .SetLink(ball.gameObject)
                .OnComplete(() =>
                {
                    _isAct = true;
                    ball.CanPick = true;
                    ball.Pick();
                });
        }

        // The basket clears CanPick when the flying ball arrives; only then does the machine close and idle.
        public override string OnUpdate(float deltaTime)
        {
            return _isAct && !_StateMachine.Blindbox.CanPick ? _StateMachine.Idle : null;
        }

        public override void OnExit()
        {
            _StateMachine.ResetBlindboxPos();
        }

        public override bool IsSuitable()
        {
            return true;
        }
    }
}
