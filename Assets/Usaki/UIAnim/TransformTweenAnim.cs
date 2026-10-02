using DG.Tweening;
using UnityEngine;

public class TransformTweenAnim : MonoBehaviour
{
    [SerializeField] protected TweenType _type;
    [SerializeField] protected Ease _ease;
    [SerializeField] protected LoopType _loopType;
    [SerializeField] protected int _loopCount;
    [SerializeField] protected float _duration;
    [SerializeField] protected bool _playOnActive;
    [SerializeField] private Vector3 _fromValue;
    [SerializeField] private Vector3 _toValue;
    protected RectTransform _rectTransform;
    private Tween _tween;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        if (_playOnActive) PlayAnim();
    }

    private void OnDisable()
    {
        _tween?.Kill();
    }

    public virtual void PlayAnim()
    {
        // Choose the animation based on the TweenType
        switch (_type)
        {
            case TweenType.Move:
                if (_rectTransform != null)
                {
                    _tween = _rectTransform
                        .DOAnchorPos(_toValue, _duration) // Tween for RectTransform (UI element)
                        .From(_fromValue)
                        .SetEase(_ease)
                        .SetLoops(_loopCount, _loopType);
                }
                else
                {
                    _tween = transform
                        .DOMove(_toValue, _duration) // Tween for Transform (non-UI element)
                        .From(_fromValue)
                        .SetEase(_ease)
                        .SetLoops(_loopCount, _loopType);
                }
                break;

            case TweenType.LocalMove:
                if (_rectTransform != null)
                {
                    _tween = _rectTransform
                        .DOAnchorPos(_toValue, _duration) // Local move for RectTransform
                        .From(_fromValue)
                        .SetEase(_ease)
                        .SetLoops(_loopCount, _loopType);
                }
                else
                {
                    _tween = transform
                        .DOLocalMove(_toValue, _duration) // Local move for Transform
                        .From(_fromValue)
                        .SetEase(_ease)
                        .SetLoops(_loopCount, _loopType);
                }
                break;

            case TweenType.Scale:
                transform.localScale = _fromValue;
                _tween = transform
                    .DOScale(_toValue, _duration) // Scale animation
                    .SetEase(_ease)
                    .SetLoops(_loopCount, _loopType);
                break;

            case TweenType.Rotate:
                _tween = transform
                    .DORotate(_toValue, _duration, RotateMode.FastBeyond360) // Rotation animation
                    .From(_fromValue)
                    .SetEase(_ease)
                    .SetLoops(_loopCount, _loopType);
                break;

            case TweenType.LocalRotate:
                _tween = transform
                    .DOLocalRotate(_toValue, _duration, RotateMode.FastBeyond360) // Local rotation animation
                    .From(_fromValue)
                    .SetEase(_ease)
                    .SetLoops(_loopCount, _loopType);
                break;

            default:
                Debug.LogWarning("TweenType not supported");
                break;
        }
    }

    public virtual void Revert()
    {
        // Reverse the tween to its original state
        _tween?.Kill(); // Kill any active tween before starting the revert
        switch (_type)
        {
            case TweenType.Move:
                if (_rectTransform != null)
                {
                    _tween = _rectTransform
                        .DOMove(_fromValue, _duration)
                        .From(_toValue)
                        .SetEase(_ease)
                        .SetLoops(_loopCount, _loopType);
                }
                else
                {
                    _tween = transform
                        .DOMove(_fromValue, _duration)
                        .From(_toValue)
                        .SetEase(_ease)
                        .SetLoops(_loopCount, _loopType);
                }
                break;

            case TweenType.LocalMove:
                if (_rectTransform != null)
                {
                    _tween = _rectTransform
                        .DOAnchorPos(_fromValue, _duration)
                        .From(_toValue)
                        .SetEase(_ease)
                        .SetLoops(_loopCount, _loopType);
                }
                else
                {
                    _tween = transform
                        .DOLocalMove(_fromValue, _duration)
                        .From(_toValue)
                        .SetEase(_ease)
                        .SetLoops(_loopCount, _loopType);
                }
                break;

            case TweenType.Scale:
                _tween = transform
                    .DOScale(_fromValue, _duration)
                    .From(_toValue)
                    .SetEase(_ease)
                    .SetLoops(_loopCount, _loopType);
                break;

            case TweenType.Rotate:
                _tween = transform
                    .DORotate(_fromValue, _duration, RotateMode.FastBeyond360)
                    .From(_toValue)
                    .SetEase(_ease)
                    .SetLoops(_loopCount, _loopType);
                break;

            case TweenType.LocalRotate:
                _tween = transform
                    .DOLocalRotate(_fromValue, _duration, RotateMode.FastBeyond360)
                    .From(_toValue)
                    .SetEase(_ease)
                    .SetLoops(_loopCount, _loopType);
                break;

            default:
                Debug.LogWarning("TweenType not supported for revert");
                break;
        }
    }

    public void KillAnim()
    {
        _tween.Kill();
    }
}
