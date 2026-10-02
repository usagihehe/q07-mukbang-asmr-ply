using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveNoFade : MonoBehaviour, IIntro, IOuttro
{
    [Header("Intro")]
    [SerializeField] protected MoveFadeSO _moveFadeIntro;
    [SerializeField] protected Ease _introEase;

    [Header("Outtro")]
    [SerializeField] protected MoveFadeSO _moveFadeOuttro;
    [SerializeField] protected Ease _outtroEase;

    public float IntroDuration => _moveFadeIntro.Duration;

    public float OuttroDuration => _moveFadeOuttro.Duration;

    public virtual bool LoadReferences()
    {
        return true;
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

        Sequence sequence = DOTween.Sequence();
        Tweener moveTween = ((RectTransform)transform).DOAnchorPos(targetPos + new Vector2(data.ToX, data.ToY), data.Duration).SetEase(ease);

        sequence.Append(moveTween);
        sequence.OnComplete(() => callbackOnComplete?.Invoke());
    }
}
