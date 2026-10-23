using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanteenProcessCtrl : ProcessCtrl
{
    [SerializeField] private TransformTweenAnim giftIcon;
    [SerializeField] private ScaleAnim _scaleAnim;
    [SerializeField] private TextMeshProUGUI _textProcess;
    private Tween _tween;
    private bool isRunLevelUp;

    [Header("TextFly")]
    [SerializeField] private TextMeshProUGUI _textFly;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _timeMove = 0.75f;
    [SerializeField] private Ease _ease;
    [SerializeField] private Vector2 _toPos = new Vector2(0, 100);
    private Vector2 _originPos;

    [Serializable]
    public class ScaleAnim
    {
        [SerializeField] public Ease ease;
        [SerializeField] public LoopType loopType;
        [SerializeField] public int loopCount;
        [SerializeField] public float duration;
        [SerializeField] public Vector3 fromValue;
        [SerializeField] public Vector3 toValue;
    }

    protected override void OnChangeValue(float value)
    {
        base.OnChangeValue(value);
        if (_textProcess != null) _textProcess.text = $"{Mathf.RoundToInt(_currentValue)}/{_maxValue}";
    }

    private void OnEnable()
    {
        MaxValue = 0;
        CurrentValue = 0;
        isRunLevelUp = false;

        //Set text
        _originPos = _textFly.rectTransform.anchoredPosition;
        _canvasGroup.alpha = 0;
        _textFly.gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        _tween?.Kill();
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public void Setup(float maxValue, float currentValue)
    {
        MaxValue = maxValue;
        LinearlyValue(currentValue, null);
    }

    public void UpdateUnlockFoodValue(float maxValue, float currentValue, float increaseValue, Action onLevelUp)
    {
        if (!isRunLevelUp) //check if running to Max to earn gift then stop run wait for open gift
        {
            Show($"+{increaseValue}");
            MaxValue = maxValue;
            LinearlyValue(currentValue, null);
        }
    }

    public void UpdateUnlockFoodLevelUp(float maxValue, float currentValue, float increaseValue, Action onLevelUp)
    {
        if (!isRunLevelUp) //check if running to Max to earn gift then stop run wait for open gift
        {
            isRunLevelUp = true;
            Show($"+{increaseValue}");
            MaxValue = maxValue;
            ChangeToMaxValue(() =>
            {
                onLevelUp?.Invoke();
                MaxValue = maxValue;
                CurrentValue = 0;
                LinearlyValue(currentValue, null);
            });
        }
    }

    private void ChangeToMaxValue(Action onComplete)
    {
        LinearlyValue(MaxValue, () =>
        {
            DisplayGift(onComplete);
        });
    }

    private void DisplayGift(Action onComplete = null)
    {
        giftIcon.KillAnim();
        _tween?.Kill();
        _tween = giftIcon.transform
                   .DOScale(_scaleAnim.toValue, _scaleAnim.duration)
                   .From(_scaleAnim.fromValue)
                   .SetEase(_scaleAnim.ease)
                   .SetLoops(_scaleAnim.loopCount, _scaleAnim.loopType)
                   .OnComplete(() =>
                   {
                       onComplete?.Invoke();
                       giftIcon.PlayAnim();
                   });
    }

    public void StopRunLeveUpProcess()
    {
        isRunLevelUp = false;
    }

    private void Show(string message)
    {
        DOTween.Kill(_canvasGroup);
        DOTween.Kill(_textFly.rectTransform);
        _textFly.gameObject.SetActive(true);
        _textFly.text = message;

        _textFly.rectTransform.anchoredPosition = _originPos;
        _canvasGroup.alpha = 0;

        _canvasGroup.DOFade(1, _timeMove * 0.5f);
        _textFly.rectTransform.DOAnchorPos(_originPos + _toPos, _timeMove).SetEase(_ease).OnComplete(() =>
        {
            _textFly.gameObject.SetActive(false);
            _textFly.rectTransform.anchoredPosition = _originPos;
        });
    }
}
