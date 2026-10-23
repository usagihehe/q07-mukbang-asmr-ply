using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEditor;

[RequireComponent(typeof(Image))]
public class LevelProcessCtrl : BaseProcess
{
    [SerializeField]
    protected Image _fill;

    [SerializeField]
    protected TextMeshProUGUI _processTxt;

    [SerializeField]
    protected TextMeshProUGUI _levelTxt;

    protected int _level;
    protected int _maxLevel;

    public virtual int Level
    {
        get => _level;
        set
        {
            _level = Mathf.Clamp(value, 0, _maxLevel);
            if (_levelTxt != null) _levelTxt.text = $"Level: {_level}";
            onChangeLevel?.Invoke(_level);
        }
    }

    public virtual int MaxLevel
    {
        get => _maxLevel;
        set => _maxLevel = Mathf.Max(1, value);
    }

    public override float MaxValue
    {
        get
        {
            return _maxValue;
        }
        set { _maxValue = value; }
    }

    public override float CurrentValue
    {
        get => _currentValue;
        set
        {
            _currentValue = Mathf.Clamp(value, 0, MaxValue);
            if (_currentValue >= MaxValue && _level < _maxLevel)
            {
                _currentValue = 0;
                Level++;
            }
            OnChangeValue(_currentValue);
            Display();
        }
    }

    public event Action<int> onChangeLevel;

    private void Start()
    {
        if (_fill == null) _fill = GetComponent<Image>();
    }

    public virtual bool Init()
    {
        Level = 0;
        CurrentValue = 0;
        return true;
    }

    protected override void OnChangeValue(float value)
    {
        _fill.fillAmount = value / MaxValue;
    }

    protected override void Display()
    {
        if (_processTxt != null) _processTxt.text = $"{(CurrentValue / MaxValue) * 100:F1}%";
    }
}
