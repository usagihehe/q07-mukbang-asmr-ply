using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotifyCtrl : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _message;

    [SerializeField]
    private CanvasGroup _canvasGroup;

    [SerializeField]
    private float _timeMove = 0.75f;

    [SerializeField]
    private Ease _ease;

    [SerializeField]
    private Vector2 _toPos = new Vector2(0, 100);

    private Vector2 _originPos;

    private RectTransform _rectTransform;

    private ContentSizeFitter _contentSizeFitter;

    // When the notify starts inactive, Awake runs inside Show's SetActive(true) and must not hide it again.
    private bool _isShowing;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _contentSizeFitter = GetComponent<ContentSizeFitter>();
        if (_contentSizeFitter == null)
        {
            _contentSizeFitter = gameObject.AddComponent<ContentSizeFitter>();
            _contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            _contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        }

        _originPos = _rectTransform.anchoredPosition;
        _canvasGroup.alpha = 0;
        if (!_isShowing) gameObject.SetActive(false);
    }

    public void Show(string message)
    {
        DOTween.Kill(transform);
        DOTween.Kill(_canvasGroup);
        DOTween.Kill(_rectTransform);
        _isShowing = true;
        gameObject.SetActive(true);
        _message.text = message;
        if (_contentSizeFitter != null)
        {
            Canvas.ForceUpdateCanvases();
        }

        _rectTransform.anchoredPosition = _originPos;
        _canvasGroup.alpha = 0;

        _canvasGroup.DOFade(1, _timeMove * 0.5f);
        _rectTransform.DOAnchorPos(_originPos + _toPos, _timeMove).SetEase(_ease).OnComplete(() =>
        {
            gameObject.SetActive(false);
            _rectTransform.anchoredPosition = _originPos;
        });
    }

}
