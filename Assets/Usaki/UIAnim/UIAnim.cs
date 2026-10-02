using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAnim : MonoBehaviour
{
    [Header("UI Animation Settings")]
    [SerializeField] private bool _playOnEnable;
    [SerializeField] private bool _blockButtonOnPlay;
    [SerializeField] private List<Button> _buttons;

    protected IIntro _intro;
    protected IOuttro _outtro;

    public IIntro Intro => _intro;
    public IOuttro Outtro => _outtro;

    protected virtual void Awake()
    {
        LoadReferences();
    }

    protected virtual void OnEnable()
    {
        if (_playOnEnable) ShowIntro(null);
    }

    public virtual bool LoadReferences()
    {
        if (_intro != null && _outtro != null) return true;

        _intro = GetComponent<IIntro>();
        _outtro = GetComponent<IOuttro>();

        return _intro != null && _outtro != null;
    }

    public virtual void ShowIntro(Action callbackOnComplete)
    {
        if (!LoadReferences()) return;

        SetButtonsInteractable(false);
        _intro.ShowIntro(() =>
        {
            SetButtonsInteractable(true);
            callbackOnComplete?.Invoke();
        });
    }

    public virtual void ShowOuttro(Action callbackOnComplete)
    {
        if (!LoadReferences()) return;

        SetButtonsInteractable(false);
        _outtro.ShowOuttro(() =>
        {
            SetButtonsInteractable(true);
            callbackOnComplete?.Invoke();
        });
    }

    private void SetButtonsInteractable(bool state)
    {
        if (!_blockButtonOnPlay) return;

        foreach (var button in _buttons)
        {
            if (button != null) button.interactable = state;
        }
    }
}
