using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using Usaki;

public class FridgeCtrl : MonoBehaviour
{
    [SerializeField]
    private Image _lightLeft;

    [SerializeField]
    private Image _lightRight;

    [SerializeField]
    [Header("Door")]
    private Button _interactBtn;

    [SerializeField]
    private Image _leftDoor;

    [SerializeField]
    private Image _rightDoor;

    [SerializeField]
    private float _moveDistance;

    [SerializeField]
    private float _timeDoor;

    [Header("Shinny")]
    [SerializeField]
    private float _delayShinny;

    [SerializeField]
    private float _timeMove;

    [SerializeField]
    private float _fromX;

    [SerializeField]
    private float _toX;

    [SerializeField]
    private Ease _moveEase;

    [SerializeField] private AudioClip _soundOpen;

    private Vector2 _originLeft;

    private Vector2 _originRight;

    private Coroutine _coroutine;

    private void Awake()
    {
        _interactBtn.onClick.AddListener(Open);
        _originLeft = _leftDoor.rectTransform.anchoredPosition;
        _originRight = _rightDoor.rectTransform.anchoredPosition;
    }

    private void OnEnable()
    {
        _interactBtn.gameObject.SetActive(true);
        _rightDoor.gameObject.SetActive(true);
        _leftDoor.gameObject.SetActive(true);
        _coroutine = StartCoroutine(Shinny());
    }

    private void OnDisable()
    {
        StopCoroutine(_coroutine);
        Close();
    }

    public void Open()
    {
        StopCoroutine(_coroutine);
        AudioManager.Instance.PlaySoundEffect(_soundOpen);
        _leftDoor.rectTransform.DOAnchorPosX(_leftDoor.rectTransform.anchoredPosition.x - _moveDistance, _timeDoor).SetEase(_moveEase).OnComplete(() =>
        {
            _interactBtn.gameObject.SetActive(false);
            _leftDoor.gameObject.SetActive(false);
        });
        _rightDoor.rectTransform.DOAnchorPosX(_rightDoor.rectTransform.anchoredPosition.x + _moveDistance, _timeDoor).SetEase(_moveEase).OnComplete(() =>
        {
            _rightDoor.gameObject.SetActive(false);
        });

    }

    public void Close()
    {
        _leftDoor.rectTransform.DOAnchorPos(_originLeft, _timeDoor).SetEase(_moveEase);
        _rightDoor.rectTransform.DOAnchorPos(_originRight, _timeDoor).SetEase(_moveEase);
    }

    public IEnumerator Shinny()
    {
        while (true)
        {
            _lightLeft.rectTransform.DOAnchorPosX(_originLeft.x + _toX, _timeMove).SetEase(_moveEase)
                .OnComplete(() =>
                {
                    _lightLeft.rectTransform.anchoredPosition = new Vector2(_originLeft.x + _fromX, _originLeft.y);
                });

            _lightRight.rectTransform.DOAnchorPosX(_originRight.x + _fromX, _timeMove).SetEase(_moveEase)
                .OnComplete(() =>
                {
                    _lightRight.rectTransform.anchoredPosition = new Vector2(_originRight.x + _toX, _originRight.y);
                });
            yield return new WaitForSeconds(_delayShinny);
        }
    }
}
