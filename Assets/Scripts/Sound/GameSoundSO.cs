using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/GameSoundSO")]
public class GameSoundSO : ScriptableObject
{
    [Header("SFX")]
    public List<AudioClip> Sounds;

    public AudioClip GetSound(string name)
    {
        return Sounds?.FirstOrDefault(s => s != null && s.name == name);
    }
}
