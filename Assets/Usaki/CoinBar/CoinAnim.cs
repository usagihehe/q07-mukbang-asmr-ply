using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Usaki
{
    public class CoinAnim : MonoBehaviour
    {
        [Header("Scale")]
        [SerializeField]
        private float _timeScale;

        [SerializeField]
        private Vector3 _toScale;

        [SerializeField]
        private Ease _easeScale;

        [Header("Move")]
        [SerializeField]
        private float _timeMove;

        [SerializeField]
        private Ease _easeMove;
        private RectTransform _rectTransform;
        private RectTransform _parent;
        private Vector2 _originPos;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _parent = _rectTransform.parent as RectTransform;
            _originPos = _rectTransform.anchoredPosition;
        }



        private void OnDisable()
        {
            transform.DOKill();
        }

        public void Spawn()
        {
            if (_rectTransform == null) _rectTransform = GetComponent<RectTransform>();
            gameObject.SetActive(true);
            _rectTransform.anchoredPosition = _originPos;
            _rectTransform.localScale = Vector3.zero;
            _rectTransform.DOScale(_toScale, _timeScale).SetEase(_easeScale);
        }

        public void MoveToTarget(Vector2 toTarget, Action callbackOnDone)
        {
            if (_rectTransform == null) _rectTransform = GetComponent<RectTransform>();
            Sequence seq = DOTween.Sequence();

            _rectTransform.DOAnchorPos(toTarget, _timeMove).SetEase(_easeMove).OnComplete(() =>
              {
                  callbackOnDone?.Invoke();
                  gameObject.SetActive(false);
              });
        }
    }
}
