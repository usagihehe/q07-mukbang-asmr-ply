using System;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;

namespace PLY33.Blindbox
{
    /// <summary>Ball on the live table: opens after a number of taps, then reports so the food can be revealed.</summary>
    public class BlindBoxItemCtrl : MonoBehaviour
    {
        [SerializeField] private SkeletonGraphic _blindBox;
        [SerializeField] private GameObject _lightEffect;
        [SerializeField] private string _animOnOpen = "animation";
        [SerializeField] private string _defaultAnim = "idle";
        [SerializeField] private float _delayTap = 0.1f;
        [SerializeField] private int _numTapToOpen = 3;

        [Header("Tap punch")]
        [SerializeField] private float _punchAmount = 0.2f;
        [SerializeField] private float _punchDuration = 0.4f;

        private int _countTap;
        private float _lastTapTime;
        private bool _isOpening;
        private Vector3 _defaultScale;

        public bool IsOpen { get; private set; }

        private void Awake()
        {
            _defaultScale = _blindBox.transform.localScale;
        }

        public void Init(SupermarketItemSO itemSo)
        {
            IsOpen = false;
            _isOpening = false;
            _countTap = 0;
            _lastTapTime = float.NegativeInfinity;
            _blindBox.gameObject.SetActive(true);
            if (_lightEffect != null) _lightEffect.SetActive(false);
            // SupermarketLivePanel.OnValidate calls Init in Edit Mode, where the Spine state is not built.
            if (!Application.isPlaying) return;

            _blindBox.transform.DOKill();
            _blindBox.transform.localScale = _defaultScale;
            BlindboxSkin.Apply(_blindBox, itemSo);
            _blindBox.AnimationState.SetAnimation(0, _defaultAnim, true);
        }

        public void OnTapBox(Action onOpened)
        {
            if (IsOpen || _isOpening || Time.time - _lastTapTime < _delayTap) return;
            _lastTapTime = Time.time;

            Transform ballTransform = _blindBox.transform;
            ballTransform.DOKill();
            ballTransform.localScale = _defaultScale;
            ballTransform.DOPunchScale(_defaultScale * _punchAmount, _punchDuration).SetLink(gameObject);

            _countTap++;
            if (_countTap >= _numTapToOpen) OpenBox(onOpened);
        }

        private void OpenBox(Action onOpened)
        {
            _isOpening = true;
            if (_lightEffect != null) _lightEffect.SetActive(true);
            _blindBox.AnimationState.SetAnimation(0, _animOnOpen, false).Complete += _ =>
            {
                _blindBox.gameObject.SetActive(false);
                if (_lightEffect != null) _lightEffect.SetActive(false);
                _isOpening = false;
                IsOpen = true;
                onOpened?.Invoke();
            };
        }
    }
}
