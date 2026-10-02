using System;

public interface IOuttro
{
    float OuttroDuration { get; }

    void ShowOuttro(Action callbackOnComplete);
}
