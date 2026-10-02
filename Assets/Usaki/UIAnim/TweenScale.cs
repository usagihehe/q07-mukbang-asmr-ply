using DG.Tweening;
using UnityEngine;

public class TweenScale : MonoBehaviour
{
    [SerializeField]
    protected Vector3 _startScale;

    [SerializeField]
    protected Vector3 _toScale;

    [SerializeField]
    private float _time;

    [SerializeField]
    private float _delayShow;

    [SerializeField]
    private Ease _easeType;

    [SerializeField]
    private bool _playOnEnable;

    private void OnEnable()
    {
        if (_playOnEnable)
            transform.DOScale(_toScale, _time).From(_startScale)
                .SetEase(_easeType).SetDelay(_delayShow);
    }

    private void OnDisable()
    {
        transform.DOKill();
    }
}
