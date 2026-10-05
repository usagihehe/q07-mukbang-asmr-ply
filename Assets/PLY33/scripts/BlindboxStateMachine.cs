using System.Collections.Generic;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;

namespace PLY33.Blindbox
{
    /// <summary>Gacha machine sequence: idle → act → gacha → open (ball waits to be picked) → idle.</summary>
    public class BlindboxStateMachine : BaseStateMachine
    {
        public const string Idle = "idle";
        public const string Act = "act";
        public const string Gacha = "gacha";
        public const string Open = "open";

        [SerializeField] private List<SupermarketItemSO> _gachaItems;
        [SerializeField] private SkeletonGraphic _model;
        [SerializeField] private BlindboxSlotShelf _blindbox;
        [Tooltip("Ball anchored Y before it drops; keep it hidden behind the machine mask.")]
        [SerializeField] private float _startPosY;

        private bool _isPlayRequested;

        public SkeletonGraphic Model => _model;
        public BlindboxSlotShelf Blindbox => _blindbox;
        public float RestPosY { get; private set; }
        public bool IsIdle => CurrentStateName == Idle;

        protected override string InitialState => Idle;

        protected override void Awake()
        {
            base.Awake();
            RestPosY = _blindbox.BallTransform.anchoredPosition.y;
        }

        protected override void OnEnable()
        {
            _isPlayRequested = false;
            ResetBlindboxPos();
            base.OnEnable();
        }

        public void RequestPlay()
        {
            _isPlayRequested = true;
        }

        public bool ConsumePlayRequest()
        {
            bool wasRequested = _isPlayRequested;
            _isPlayRequested = false;
            return wasRequested;
        }

        public SupermarketItemSO GetRandomItem()
        {
            return _gachaItems[Random.Range(0, _gachaItems.Count)];
        }

        public void ResetBlindboxPos()
        {
            RectTransform ball = _blindbox.BallTransform;
            ball.DOKill();
            ball.anchoredPosition = new Vector2(ball.anchoredPosition.x, _startPosY);
            _blindbox.CanPick = false;
        }
    }
}
