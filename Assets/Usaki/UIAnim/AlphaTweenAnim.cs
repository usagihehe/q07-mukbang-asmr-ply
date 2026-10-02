using DG.Tweening;
using System;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class AlphaTweenAnim : MonoBehaviour
{
    [SerializeField]
    protected Ease _ease;

    [SerializeField]
    protected int _loopCount;

    [SerializeField]
    protected float _duration;

    [SerializeField]
    protected bool _playOnActive;

    [SerializeField]
    private float _from;

    [SerializeField]
    private float _to;

    private CanvasGroup _canvasGroup;

    public float Duration => _duration;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = _from;
    }

    private void OnEnable()
    {
        if (_playOnActive)
        {
            PlayAnim();
        }
    }

    public virtual void PlayAnim(Action callbackOnComplete = null)
    {
        if (_canvasGroup == null) _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.DOFade(_to, _duration)
            .SetEase(_ease).From(_from)
            .SetLoops(_loopCount, LoopType.Yoyo).OnComplete(() => callbackOnComplete?.Invoke());
    }

    public virtual void Revert(Action callbackOnComplete = null)
    {
        _canvasGroup.DOFade(_from, _duration)
              .SetEase(_ease).From(_to)
              .SetLoops(_loopCount, LoopType.Yoyo).OnComplete(() => callbackOnComplete?.Invoke());
    }
}
