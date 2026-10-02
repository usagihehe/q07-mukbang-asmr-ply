using UnityEngine;

[CreateAssetMenu(menuName = "SO/UI/MoveFade")]
public class MoveFadeSO : ScriptableObject
{
    [Header("Informations")]
    public float Duration;

    public bool Relative;

    [Header("Fade")]
    public float FromFade;

    public float ToFade;

    [Header("Position")]
    public bool UseX;

    public float FromX;

    public float ToX;

    [Space(10f)]
    public bool UseY;

    public float FromY;

    public float ToY;
}
