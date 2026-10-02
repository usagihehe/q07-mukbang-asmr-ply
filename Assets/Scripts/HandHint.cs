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

        private Tween _tween;

        private void Awake()
        {
            _hand.DOKill();
        }

        public void SetHintOn(bool isActive)
        {
            if (!isActive) _tween?.Kill();
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

        public void StopHint()
        {
            _tween?.Kill();
            _hand.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            _tween?.Kill();
        }
    }
}
