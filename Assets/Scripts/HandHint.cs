using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Usaki
{
    public class HandHint : MonoBehaviour
    {
        [SerializeField] private RectTransform _hand;
        [SerializeField] private Vector2 _clickOffset = new Vector2(60f, -84f);
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private float _moveDuration = 0.6f;
        [SerializeField] private Ease _ease = Ease.InOutSine;
        [SerializeField] private float _pointStayDuration = 1.5f;
        [SerializeField] private float _pointedScaleMultiplier = 1.1f;

        private Tween _tween;
        private Tween _pulseTween;
        private Transform _pointedTarget;
        private Vector3 _pointedBaseScale;

        private void Awake()
        {
            _hand.DOKill();
        }

        public void SetHintOn(bool isActive)
        {
            if (!isActive)
            {
                _tween?.Kill();
                StopPulse();
            }
            _hand.gameObject.SetActive(isActive);
        }

        private void ShowHint(Vector2 anchoredPosition)
        {
            _hand.DOKill();
            _hand.anchoredPosition = anchoredPosition;
            _hand.gameObject.SetActive(true);
            _tween = _hand.DOAnchorPos(anchoredPosition + _clickOffset, _duration)
                .SetEase(_ease)
                .SetLoops(-1, LoopType.Yoyo);
        }

        public void ShowHint(Transform target, Canvas canvas)
        {
            ShowHint(RectTransformExtension.CanvasLocalPoint(target, canvas));
        }

        private void MoveHint(Vector2 from, Vector2 to)
        {
            _hand.DOKill();
            _hand.anchoredPosition = from;
            _hand.gameObject.SetActive(true);
            _tween = _hand.DOAnchorPos(to, _moveDuration)
                .SetEase(_ease)
                .SetLoops(-1, LoopType.Restart);
        }

        public void MoveHint(Transform from, Transform to, Canvas canvas)
        {
            MoveHint(
                RectTransformExtension.CanvasLocalPoint(from, canvas),
                RectTransformExtension.CanvasLocalPoint(to, canvas));
        }

        public void PointSequentially(IReadOnlyList<Transform> targets, Canvas canvas)
        {
            _tween?.Kill();
            _hand.DOKill();
            _hand.gameObject.SetActive(true);
            Sequence sequence = DOTween.Sequence();
            foreach (Transform target in targets)
            {
                Vector3 baseScale = target.localScale;
                sequence.AppendCallback(() => PointAt(target, baseScale, canvas));
                sequence.AppendInterval(_pointStayDuration);
            }
            _tween = sequence.SetLoops(-1);
        }

        private void PointAt(Transform target, Vector3 baseScale, Canvas canvas)
        {
            StopPulse();
            _hand.DOAnchorPos(RectTransformExtension.CanvasLocalPoint(target, canvas), _moveDuration)
                .SetEase(_ease)
                .OnComplete(() => StartPulse(target, baseScale));
        }

        private void StartPulse(Transform target, Vector3 baseScale)
        {
            _pointedTarget = target;
            _pointedBaseScale = baseScale;
            _pulseTween = target.DOScale(baseScale * _pointedScaleMultiplier, _duration)
                .SetEase(_ease)
                .SetLoops(-1, LoopType.Yoyo);
        }

        private void StopPulse()
        {
            _hand.DOKill();
            _pulseTween?.Kill();
            if (_pointedTarget != null) _pointedTarget.DOScale(_pointedBaseScale, _duration).SetEase(_ease);
            _pointedTarget = null;
        }

        public void StopHint()
        {
            _tween?.Kill();
            StopPulse();
            _hand.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            _tween?.Kill();
            _pulseTween?.Kill();
        }
    }
}
