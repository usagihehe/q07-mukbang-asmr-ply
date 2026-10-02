using System;
using DG.Tweening;
using UnityEngine;

namespace Supermarket
{
    /// <summary>Tween-driven claw crane: looping left-right sweep, plus a drop-grab-lift pick.</summary>
    public class LuckyControlTween : MonoBehaviour
    {
        [Header("Rig")]
        [SerializeField] private RectTransform _slider;
        [SerializeField] private RectTransform _rope;

        [Header("Sweep")]
        [SerializeField] private float _leftX = -216f;
        [SerializeField] private float _rightX = 213.33f;
        [SerializeField] private float _sweepDuration = 1.4f;

        [Header("Pick")]
        [SerializeField] private float _ropeIdleHeight = 70f;
        [SerializeField] private float _ropeGrabHeight = 281.44f;
        [SerializeField] private float _dropDuration = 0.42f;
        [SerializeField] private float _liftDuration = 0.42f;

        private Sequence _idleSequence;
        private Sequence _pickSequence;

        public bool IsPicking => _pickSequence != null && _pickSequence.IsActive();

        private void OnDisable() => KillTweens();

        public void PlayIdle()
        {
            KillTweens();
            _slider.anchoredPosition = new Vector2(_rightX, _slider.anchoredPosition.y);
            _rope.sizeDelta = RopeSize(_ropeIdleHeight);

            _idleSequence = DOTween.Sequence()
                .Append(_slider.DOAnchorPosX(_leftX, _sweepDuration).SetEase(Ease.InOutSine))
                .Append(_slider.DOAnchorPosX(_rightX, _sweepDuration).SetEase(Ease.InOutSine))
                .SetLoops(-1);
        }

        /// <summary>Drops the claw from wherever the sweep currently is; onGrab fires at full extension.</summary>
        public void PlayPick(Action onGrab)
        {
            if (IsPicking) return;

            _idleSequence?.Pause();
            _pickSequence = DOTween.Sequence()
                .Append(_rope.DOSizeDelta(RopeSize(_ropeGrabHeight), _dropDuration).SetEase(Ease.InQuad))
                .AppendCallback(() => onGrab?.Invoke())
                .Append(_rope.DOSizeDelta(RopeSize(_ropeIdleHeight), _liftDuration).SetEase(Ease.OutQuad))
                .OnComplete(ResumeIdle);
        }

        private void ResumeIdle()
        {
            _pickSequence = null;
            _idleSequence?.Play();
        }

        private void KillTweens()
        {
            _idleSequence?.Kill();
            _pickSequence?.Kill();
            _idleSequence = null;
            _pickSequence = null;
        }

        private Vector2 RopeSize(float height) => new Vector2(_rope.sizeDelta.x, height);
    }
}
