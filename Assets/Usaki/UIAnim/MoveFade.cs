using DG.Tweening;
using System;
using UnityEngine;
public class MoveFade : MonoBehaviour, IIntro, IOuttro
{
    [SerializeField]
    [Header("Intro")]
    protected MoveFadeSO _moveFadeIntro;

    [SerializeField]
    protected Ease _introEase;

    [SerializeField]
    [Header("Outtro")]
    protected MoveFadeSO _moveFadeOuttro;

    [SerializeField]
    protected Ease _outtroEase;

    protected CanvasGroup _canvasGroup;

    public float IntroDuration => _moveFadeIntro.Duration;

    public float OuttroDuration => _moveFadeOuttro.Duration;

    public virtual bool LoadReferences()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        return _canvasGroup != null;
    }

    public virtual void ShowIntro(Action callbackOnComplete)
    {
        LoadReferences();
        ((RectTransform)transform).anchoredPosition = Vector2.zero;
        Play(_moveFadeIntro, _introEase, callbackOnComplete);
    }

    public virtual void ShowOuttro(Action callbackOnComplete)
    {
        LoadReferences();
        ((RectTransform)transform).anchoredPosition = Vector2.zero;
        Play(_moveFadeOuttro, _outtroEase, callbackOnComplete);
    }

    protected virtual void Play(MoveFadeSO data, Ease ease, Action callbackOnComplete)
    {
        Vector2 targetPos = ((RectTransform)transform).anchoredPosition;
        Vector2 startPos = targetPos;

        if (data.UseX) startPos.x = data.Relative ? targetPos.x + data.FromX : data.FromX;
        if (data.UseY) startPos.y = data.Relative ? targetPos.y + data.FromY : data.FromY;

        ((RectTransform)transform).anchoredPosition = startPos;
        _canvasGroup.alpha = /*data.Relative ?*/ data.FromFade /*: 0*/;

        Sequence sequence = DOTween.Sequence();
        Tweener moveTween = ((RectTransform)transform).DOAnchorPos(targetPos + new Vector2(data.ToX, data.ToY), data.Duration).SetEase(ease);
        Tweener fadeTween = _canvasGroup.DOFade(data.ToFade, data.Duration).SetEase(ease);

        sequence.Append(moveTween);
        sequence.Join(fadeTween);
        sequence.OnComplete(() => callbackOnComplete?.Invoke());
    }
}
