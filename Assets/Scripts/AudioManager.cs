using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Utils.Singletons;

public class AudioManager : ZMonoSingleton<AudioManager>
{
    public const string KEY_MUSIC = "Music";
    public const string KEY_SOUND = "Sound";
    public const string KEY_VIBRATION = "VIBRATION";

    [Header("Audio Mixer")] public AudioMixer audioMixer;
    [Header("Audio Sources")]
    [SerializeField] private AudioSource audioMain;
    [SerializeField] private AudioSource audioGamePlay;
    [SerializeField] private AudioSource audioSoundEffect;
    [SerializeField] private AudioSource audioSoundEffectLoop;
    [SerializeField] private AudioSource audioClick;
    [SerializeField] private AudioSource audioSourceShakeDrink;

    private bool isPlayingMainMusic;

    protected override void Awake()
    {
        base.Awake();
        float musicVolume = PlayerPrefs.GetFloat(KEY_MUSIC, 1f);
        float soundVolume = PlayerPrefs.GetFloat(KEY_SOUND, 1f);

        SetMusic(musicVolume);
        SetSound(soundVolume);

        // Default state
        isPlayingMainMusic = false;
    }

    public void PlaySoundEffect(AudioClip _audio)
    {
        if (_audio == null) return;

        float volume = GetSoundVolumn();
        if (volume > 0)
        {
            audioSoundEffect.PlayOneShot(_audio);
        }
    }

    public void StopSoundEffect()
    {
        audioSoundEffect.Stop();
    }

    public void SetMusic(float volume)
    {
        SetStatusAudio(KEY_MUSIC, volume);
        SetMusicVolumn(volume);
    }

    public void SetSound(float volume)
    {
        SetStatusAudio(KEY_SOUND, volume);
        SetSoundVolumn(volume);
    }

    private void SetStatusAudio(string nameAudio, float volume)
    {
        PlayerPrefs.SetFloat(nameAudio, volume);
        PlayerPrefs.Save();
    }

    public void PlayMusicMain()
    {
        if (!isPlayingMainMusic)
        {
            audioMain.Stop();
            if (audioMain.clip != null)
            {
                audioMain.Play();
                isPlayingMainMusic = true;
            }
        }
    }

    public void PlayMusicGamePlay(AudioClip sound)
    {
        if (sound == null) return;
        audioGamePlay.Stop();
        audioGamePlay.loop = false;
        audioGamePlay.clip = sound;
        audioGamePlay.Play();
    }

    public void StopMusicGamePlay()
    {
        audioGamePlay.Stop();
    }

    public void PlayBGM(AudioClip sound)
    {
        if (sound == null) return;

        audioMain.Stop();
        isPlayingMainMusic = false;
        audioMain.clip = sound;
        audioMain.Play();
    }

    public void StopBGM()
    {
        audioMain.Stop();
        isPlayingMainMusic = false;
    }

    public void PlaySoundLoop(AudioClip sound)
    {
        if (sound == null) return;

        audioSoundEffectLoop.clip = sound;
        audioSoundEffectLoop.Play();
    }

    public void StopSoundLoop()
    {
        audioSoundEffectLoop.Stop();
    }

    public void PlayShakeEffect(AudioClip _audio)
    {
        if (_audio == null) return;

        float volume = GetSoundVolumn();
        if (volume > 0 && !audioSourceShakeDrink.isPlaying)
        {
            audioSourceShakeDrink.PlayOneShot(_audio);
        }
    }

    public void StopShakeEffect()
    {
        audioSourceShakeDrink.Stop();
    }

    public void SetMusicVolumn(float volume)
    {
        // Convert volume value (0-1) to decibels for AudioMixer
        // -80dB is effectively silent
        float dB = volume > 0.001f ? 20f * Mathf.Log10(volume) : -80f;
        audioMixer.SetFloat("Music", dB);
    }

    public void SetSoundVolumn(float volume)
    {
        // Convert volume value (0-1) to decibels for AudioMixer
        // -80dB is effectively silent
        float dB = volume > 0.001f ? 20f * Mathf.Log10(volume) : -80f;
        audioMixer.SetFloat("Sound", dB);
    }

    public float GetMusicVolumn()
    {
        float volume = PlayerPrefs.GetFloat(KEY_MUSIC, 1f);
        return volume;
    }

    public float GetSoundVolumn()
    {
        float volume = PlayerPrefs.GetFloat(KEY_SOUND, 1f);
        return volume;
    }

    public bool CheckOnVibration()
    {
        return PlayerPrefs.GetInt(KEY_VIBRATION, 1) == 1;
    }

    public void SetVibration(bool _on)
    {
        PlayerPrefs.SetInt(KEY_VIBRATION, _on ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void PlayAudioClick()
    {
        float volume = GetSoundVolumn();
        if (volume > 0 && audioClick.clip != null)
        {
            audioClick.Play();
        }
    }

}
