using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/CharacterSound")]
public class CharacterSoundSO : ScriptableObject
{
    [SerializeField] private List<SoundAudio> EatSounds;
    [SerializeField] private List<EmotionAudio> EmotionSounds;

    public AudioClip GetEatSound(SoundType type)
    {
        if (EmotionSounds == null) return null;
        foreach (var sound in EatSounds)
        {
            if (sound.Type == type) return sound.AudioClip;
        }
        Debug.LogWarning($"Eat sound not found for type: {type}");
        return null;
    }

    public AudioClip GetEmotionSound(EmotionType type)
    {
        if (EmotionSounds == null) return null;
        foreach (var sound in EmotionSounds)
        {
            if (sound.Type == type) return sound.AudioClip;
        }

        Debug.LogWarning($"Emotion sound not found for type: {type}");
        return null;
    }
}

[Serializable]
public class SoundAudio
{
    public SoundType Type;
    public AudioClip AudioClip;
}

[Serializable]
public class EmotionAudio
{
    public EmotionType Type;
    public AudioClip AudioClip;
}
