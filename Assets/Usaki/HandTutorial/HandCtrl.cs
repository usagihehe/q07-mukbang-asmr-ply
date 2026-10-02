using DG.Tweening;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Usaki
{
    public class HandCtrl : MonoBehaviour
    {
        [SerializeField] protected SkeletonGraphic _hand;
        [SerializeField] protected RectTransform _handRect;
        [SerializeField] private Ease _ease;
        [SerializeField] private LoopType _loopType;
        [SerializeField] private int _loopCount;
        [SerializeField] private float _duration;

        private Transform Transform => gameObject.transform;
        private Sequence _sequence;
        private Transform _dynamicClickTarget;
        private Canvas _dynamicClickCanvas;
        protected const string POINT = "tut_point";

        private void Update()
        {
            if (_dynamicClickTarget != null && _dynamicClickCanvas != null)
                _hand.transform.localPosition = RectTransformExtension.CanvasLocalPoint(_dynamicClickTarget, _dynamicClickCanvas);
        }

        public void SetHandOn(bool isActive)
        {
            _hand.gameObject.SetActive(isActive);
        }

        public void StopHand()
        {
            _sequence?.Kill();
            _sequence = null;
            _dynamicClickTarget = null;
            _dynamicClickCanvas = null;
            _hand.gameObject.SetActive(false);
        }

        public void ClickAnim(Vector3 localPosition, float delay = 0)
        {
            _sequence?.Kill();
            _dynamicClickTarget = null;
            _dynamicClickCanvas = null;
            _hand.transform.localPosition = localPosition;
            _hand.gameObject.SetActive(true);
            _hand.AnimationState.TimeScale = 1;
            _hand.AnimationState.SetAnimation(0, POINT, true);
        }

        public void ClickAnim(Transform target, Canvas canvas)
        {
            _sequence?.Kill();
            _dynamicClickTarget = target;
            _dynamicClickCanvas = canvas;
            _hand.gameObject.SetActive(true);
            _hand.AnimationState.TimeScale = 1;
            _hand.AnimationState.SetAnimation(0, POINT, true);
        }

        public void PlayMoveHand(Vector3 start, Vector3 target)
        {
            _sequence?.Kill();
            _dynamicClickTarget = null;
            _dynamicClickCanvas = null;
            _hand.AnimationState.TimeScale = 0;
            _hand.transform.localPosition = start;
            _sequence = DOTween.Sequence();
            _sequence.Append(_hand.transform.DOLocalMove(target, _duration).SetEase(_ease));
            _sequence.AppendInterval(0.25f);
            _sequence.SetLoops(_loopCount, _loopType);
        }

        public void PlacyMcoveHand(Transform from, Transform to, Canvas canvas)
        {
            _sequence?.Kill();
            _dynamicClickTarget = null;
            _dynamicClickCanvas = null;
            _hand.gameObject.SetActive(true);
            _hand.AnimationState.TimeScale = 0;

            float t = 0f;
            _sequence = DOTween.Sequence();
            _sequence.Append(
                DOTween.To(() => t, x =>
                {
                    t = x;
                    Vector3 start = RectTransformExtension.CanvasLocalPoint(from, canvas);
                    Vector3 end   = RectTransformExtension.CanvasLocalPoint(to, canvas);
                    _hand.transform.localPosition = Vector3.Lerp(start, end, t);
                }, 1f, _duration).SetEase(_ease)
            );
            _sequence.AppendInterval(0.25f);
            _sequence.SetLoops(_loopCount, _loopType);
        }
    }
}