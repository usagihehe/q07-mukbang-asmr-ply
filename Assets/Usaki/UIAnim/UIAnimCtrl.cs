using System;

public class UIAnimCtrl
{
    private int _maxCount;

    private int _curCount;

    private UIAnim[] _anims;

    private Action _callbackOnDone;

    public UIAnimCtrl(UIAnim[] anims)
    {
        _anims = anims;
        _maxCount = anims.Length;
        _curCount = 0;
    }

    public void PlayIntro(Action callOnDone)
    {
        if (_anims == null || _anims.Length == 0)
        {
            callOnDone?.Invoke();
            return;
        }

        _callbackOnDone = callOnDone;
        _curCount = 0;
        foreach (var anim in _anims)
        {
            anim.ShowIntro(OnDone);
        }
    }

    public void PlayOuttro(Action callOnDone)
    {
        if (_anims == null || _anims.Length == 0)
        {
            callOnDone?.Invoke();
            return;
        }

        _callbackOnDone = callOnDone;
        _curCount = 0;
        foreach (var anim in _anims)
        {
            anim.ShowOuttro(OnDone);
        }
    }

    private void OnDone()
    {
        _curCount++;
        if (_curCount >= _maxCount)
        {
            _callbackOnDone?.Invoke();
            _curCount = 0;
        }
    }
}
