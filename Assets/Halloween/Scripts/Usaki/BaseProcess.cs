using DG.Tweening;
using System;
using UnityEngine;

public abstract class BaseProcess : MonoBehaviour
{
    protected float _minValue;

    protected float _currentValue;

    protected float _maxValue;

    public virtual float MinValue { get; set; }

    public virtual float CurrentValue
    {
        get
        {
            return _currentValue;
        }
        set
        {
            _currentValue = value;
        }
    }

    public virtual float MaxValue
    {
        get
        {
            return _maxValue;
        }
        set
        {
            _maxValue = value;
        }
    }

    public virtual void LinearlyValue(float newValue, Action callbackOnComplete)
    {
        DOTween.To(() => _currentValue, x =>
        {
            _currentValue = x;
            OnChangeValue(_currentValue);
            Display();
        }, newValue, 1.0f) // 1.0f is the duration, change as needed
       .OnComplete(() => callbackOnComplete?.Invoke());
    }

    protected abstract void OnChangeValue(float value);

    protected virtual void Display()
    {
        Debug.Log($"Current Value: {_currentValue}");
    }
}
