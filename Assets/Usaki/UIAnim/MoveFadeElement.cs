using DG.Tweening;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;
public class MoveFadeElement : MonoBehaviour, IIntro, IOuttro
{
    [SerializeField][Header("Intro")] protected MoveFadeSO _moveFadeIntro;
    [SerializeField] protected Ease _introEase;
    [Header("Outtro")][SerializeField] protected MoveFadeSO _moveFadeOuttro;
    [SerializeField] protected Ease _outtroEase;
    [SerializeField]
    [Range(0f, 1f)]
    protected float _transTime;
    private List<Tuple<RectTransform, CanvasGroup>> _children = new List<Tuple<RectTransform, CanvasGroup>>();
    private Coroutine _coroutine;
    private bool _isLoaded;
    public float IntroDuration => _moveFadeIntro.Duration;
    public float OuttroDuration => _moveFadeOuttro.Duration;

    public void Intro()
    {
        _isLoaded = LoadReferences();
        if (_isLoaded) ShowIntro(null);
    }

    public void Outtro()
    {
        _isLoaded = LoadReferences();
        if (_isLoaded) ShowOuttro(null);
    }

    public virtual bool LoadReferences()
    {
        if (_isLoaded) return true;

        _children.Clear();
        foreach (Transform child in transform)
        {
            RectTransform rect = child.GetComponent<RectTransform>();
            CanvasGroup canvas = child.GetComponent<CanvasGroup>();
            if (rect != null && canvas != null)
            {
                _children.Add(new Tuple<RectTransform, CanvasGroup>(rect, canvas));
            }
        }

        _isLoaded = _children.Count > 0;
        return _isLoaded;
    }

    public void ShowIntro(Action callbackOnComplete)
    {
        _isLoaded = LoadReferences();
        if (!_isLoaded) return;
        if (_coroutine != null) StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(MoveElements(_children, _moveFadeIntro, _introEase, callbackOnComplete));
    }

    public void ShowOuttro(Action callbackOnComplete)
    {
        _isLoaded = LoadReferences();
        if (!_isLoaded) return;
        if (_coroutine != null) StopCoroutine(_coroutine);
        //_coroutine = StartCoroutine(MoveElements(_children, _moveFadeOuttro, _outtroEase, callbackOnComplete));
    }

    private IEnumerator MoveElements(List<Tuple<RectTransform, CanvasGroup>> children, MoveFadeSO data, Ease ease, Action callbackOnComplete)
    {
        foreach (var (rect, canvas) in children)
        {
            Vector2 basePos = rect.anchoredPosition;
            Vector2 startPos = basePos;
            Vector2 endPos = basePos;

            if (data.Relative)
            {
                if (data.UseX)
                {
                    startPos.x = basePos.x + data.FromX;
                    endPos.x = basePos.x + data.ToX;
                }
                if (data.UseY)
                {
                    startPos.y = basePos.y + data.FromY;
                    endPos.y = basePos.y + data.ToY;
                }
            }
            else
            {
                if (data.UseX)
                {
                    startPos.x = data.FromX;
                    endPos.x = data.ToX;
                }
                if (data.UseY)
                {
                    startPos.y = data.FromY;
                    endPos.y = data.ToY;
                }
            }

            rect.anchoredPosition = startPos;
            canvas.alpha = data.Relative ? 0 : data.FromFade;
            //rect.gameObject.SetActive(true); // optional
        }

        Sequence sequence = DOTween.Sequence();

        for (int i = 0; i < _children.Count; i++)
        {
            var (rect, canvas) = _children[i];
            float delay = i * _transTime;

            Vector2 targetPos;
            if (data.Relative)
            {
                Vector2 basePos = rect.anchoredPosition; // already at start
                targetPos = basePos;
                if (data.UseX) targetPos.x = basePos.x + (data.ToX - data.FromX);
                if (data.UseY) targetPos.y = basePos.y + (data.ToY - data.FromY);
            }
            else
            {
                targetPos = new Vector2(
                    data.UseX ? data.ToX : rect.anchoredPosition.x,
                    data.UseY ? data.ToY : rect.anchoredPosition.y
                );
            }

            Tweener moveTween = rect.DOAnchorPos(targetPos, data.Duration).SetEase(ease);
            Tweener fadeTween = canvas.DOFade(data.ToFade, data.Duration).SetEase(ease);

            sequence.Insert(delay, moveTween);
            sequence.Insert(delay, fadeTween);
        }

        sequence.OnComplete(() => callbackOnComplete?.Invoke());

        yield return sequence.WaitForCompletion();
    }

}