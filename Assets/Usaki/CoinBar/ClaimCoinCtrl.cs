using DG.Tweening;
using Usaki;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Usaki
{
    public class ClaimCoinCtrl : MonoBehaviour
    {
        [SerializeField] private AudioClip _claimSound;
        [Header("Coin")][SerializeField] private Image _overlay;
        [SerializeField] private RectTransform _holder;
        [SerializeField] protected float _delaySpawn;
        [SerializeField] protected float _waitMove;

        [Header("Coin Txt")]
        [SerializeField] private TextMeshProUGUI _coinTxt;
        [SerializeField] private float _timeMove;
        [SerializeField] private float _moveY;
        [SerializeField] private Ease _easeMove;
        [SerializeField] private List<CoinAnim> _coins;

        private Coroutine _spawnCoroutine;
        private Coroutine _moveCoroutine;
        private int _countDone;
        private Action _callOnDone;
        private Action _onFirstFinish;

        protected virtual void Awake()
        {
            foreach (var coin in _coins)
            {
                if (coin != null)
                    coin.gameObject.SetActive(false);
            }
        }

        protected virtual void OnDisable()
        {
            if (_spawnCoroutine != null) StopCoroutine(_spawnCoroutine);
            if (_moveCoroutine != null) StopCoroutine(_moveCoroutine);
        }

        public virtual void PlayAnim(Vector3 fromPos, Vector3 toTarget, int coin, bool showCoin, bool disableTouch = true, Action onFirstFinish = null, Action callOnDone = null)
        {
            _countDone = 0;
            gameObject.SetActive(true);
            if (showCoin) _coinTxt.text = $"+{coin}";
            _holder.transform.position = fromPos;
            _coinTxt.transform.position = fromPos;

            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _holder,
                Camera.main.WorldToScreenPoint(toTarget),
                Camera.main,
                out localPoint);
            _moveCoroutine = StartCoroutine(MoveCoin(localPoint));
            _overlay.enabled = disableTouch;
            _onFirstFinish = onFirstFinish;
            _callOnDone = callOnDone;
        }

        protected virtual IEnumerator SpawnCoin()
        {
            foreach (var coin in _coins)
            {
                coin.Spawn();
                yield return new WaitForSeconds(_delaySpawn);
            }
        }

        protected virtual IEnumerator MoveCoin(Vector2 toPos)
        {
            RectTransform rect = _coinTxt.rectTransform;
            rect.DOAnchorPosY(rect.anchoredPosition.y + _moveY, _timeMove)
                .SetEase(_easeMove);
            _spawnCoroutine = StartCoroutine(SpawnCoin());
            yield return new WaitForSeconds(_waitMove);
            foreach (var coin in _coins)
            {
                coin.MoveToTarget(toPos, OnCoinMoveDone);
                yield return new WaitForSeconds(_delaySpawn);
            }
        }

        protected virtual void OnCoinMoveDone()
        {
            AudioManager.Instance.PlaySoundEffect(_claimSound);
            _countDone++;
            if (_countDone == 1) _onFirstFinish?.Invoke();
            if (_countDone >= _coins.Count)
            {
                _callOnDone?.Invoke();
                _overlay.enabled = true;
                gameObject.SetActive(false);
            }
        }
    }
}
