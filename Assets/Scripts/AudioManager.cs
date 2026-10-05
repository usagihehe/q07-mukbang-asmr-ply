using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.Singletons;

public class AudioManager : ZMonoSingleton<AudioManager>
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource audioMain;
    [SerializeField] private AudioSource audioGamePlay;
    [SerializeField] private AudioSource audioSoundEffect;
    [SerializeField] private AudioSource audioSoundEffectLoop;
    [SerializeField] private AudioSource audioClick;

    private bool isPlayingMainMusic;

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

    public void SetMusicVolumn(float volume)
    {
        audioMain.volume = volume;
        audioGamePlay.volume = volume;
    }

    public void SetSoundVolumn(float volume)
    {
        audioSoundEffect.volume = volume;
        audioSoundEffectLoop.volume = volume;
        audioClick.volume = volume;
    }

    public float GetMusicVolumn()
    {
        return audioMain.volume;
    }

    public float GetSoundVolumn()
    {
        return audioSoundEffect.volume;
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
