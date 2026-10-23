using System;
using UnityEngine;
using UnityEngine.UI;

public class ProcessCtrl : BaseProcess
{
    public Action onDone;
    [SerializeField]
    private Image _fill;

    public override float CurrentValue
    {
        get
        {
            return _currentValue;
        }
        set
        {
            OnChangeValue(value);
            _currentValue = value;
        }
    }

    protected override void OnChangeValue(float value)
    {
        if (_fill != null)
        {
            _fill.fillAmount = value / MaxValue;
        }
    }

    protected override void Display()
    {
        //Debug.Log($"Process progress: {_currentValue} / {MaxValue}");
    }
}
