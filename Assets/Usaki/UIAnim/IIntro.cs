using System;

public interface IIntro
{
    float IntroDuration { get; }

    void ShowIntro(Action callbackOnComplete);
}
