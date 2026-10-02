using System;

public interface IPanel
{
    void ShowIntro(Action onCompleted);

    void ShowOuttro(Action onCompleted);
}
