using Spine;
using UnityEngine;

public class AfterEatState : CharBaseState
{
    private bool _isPlay;
    private TrackEntry _entry;

    public override void EnterState()
    {
        _isPlay = true;
        EmotionType emotion = _stateMachine.Brain.EatenEmotion;
        AudioClip sound = GameManager.Instance.CharSound.GetEmotionSound(emotion);
        if (sound != null) AudioManager.Instance.PlaySoundEffect(sound);
        _entry = _character.PlayAnimByName(GetAnimEmotion(emotion), false, delegate
        {
            _isPlay = false;
            _character.PlayAnimByName(CharacterCtrl.IDLE, true);
            AudioManager.Instance.StopSoundEffect();
        });
    }

    public override string Execute(float dt)
    {
        // Another script replaced our animation, so its Complete callback will never run.
        if (_isPlay && !_character.IsCurrent(_entry))
        {
            _isPlay = false;
            AudioManager.Instance.StopSoundEffect();
        }
        return _isPlay ? StateMachine.AfterEatState : StateMachine.IdleState;
    }

    public override void ExitState()
    {
    }

    public override bool IsSuitable()
    {
        return true;
    }

    private static string GetAnimEmotion(EmotionType emotionType)
    {
        switch (emotionType)
        {
            case EmotionType.Blow: return CharacterCtrl.EMOJI_BLOW;
            case EmotionType.Burp: return CharacterCtrl.EMOJI_BURP;
            case EmotionType.Cool: return CharacterCtrl.EMOJI_COOL;
            case EmotionType.Cute: return CharacterCtrl.EMOJI_CUTE;
            case EmotionType.Demon: return CharacterCtrl.EMOJI_DEMON;
            case EmotionType.Drink: return CharacterCtrl.EMOJI_DRINK;
            case EmotionType.Drooling: return CharacterCtrl.EMOJI_DROOLING;
            case EmotionType.Freeze: return CharacterCtrl.EMOJI_FREEZE;
            case EmotionType.Fresh: return CharacterCtrl.EMOJI_FRESH;
            case EmotionType.Happy: return CharacterCtrl.EMOJI_HAPPY;
            case EmotionType.Licklips: return CharacterCtrl.EMOJI_LICKLIPS;
            case EmotionType.Rainbow: return CharacterCtrl.EMOJI_RAINBOW;
            case EmotionType.Relax: return CharacterCtrl.EMOJI_RELAX;
            case EmotionType.Shock: return CharacterCtrl.EMOJI_SHOCK;
            case EmotionType.Sour: return CharacterCtrl.EMOJI_SOUR;
            case EmotionType.Spicy: return CharacterCtrl.EMOJI_SPICY;
            default: return CharacterCtrl.IDLE;
        }
    }
}
