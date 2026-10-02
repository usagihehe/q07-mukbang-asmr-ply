using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/MukbangItem")]
public class MukbangItemSO : ScriptableObject
{
    public EmotionType Emotion;
    public SoundType EatSound;
    public List<Sprite> Steps;
}
