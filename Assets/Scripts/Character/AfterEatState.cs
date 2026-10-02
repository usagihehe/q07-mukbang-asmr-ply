using DG.Tweening.Core.Easing;
using Usaki;
using UnityEngine;

public class AfterEatState : CharBaseState
{
    private bool _isPlay;
    private ItemMukbang _itemMukbang;
    public override void EnterState()
    {
        _isPlay = true;
        _itemMukbang = _stateMachine.MukbangItem;
        AudioManager.Instance.PlaySoundEffect(GetSound());
        _character.PlayAnimByName(GetEmotion(), false, delegate
        {
            _isPlay = false;
            _character.PlayAnimByName(CharacterCtrl.IDLE, true);
            AudioManager.Instance.StopSoundEffect();
        });
    }

    private AudioClip GetSound()
    {
        return GameManager.Instance.CharSound.GetEmotionSound(_itemMukbang.GetEmotion());
    }

    private string GetEmotion()
    {
        return GetAnimEmotion(_itemMukbang.GetEmotion());

    }

    public override string Execute(float dt)
    {
        if (_isPlay)
        {
            return StateMachine.AfterEatState;
        }
        else if (!_stateMachine.IsSelectItem)
        {
            return StateMachine.IdleState;
        }
        else { return StateMachine.BeforeEatState; }
    }

    public override void ExitState()
    {
    }

    private string GetAnimEmotion(EmotionType emotionType)
    {
        string animName = CharacterCtrl.IDLE;
        switch (emotionType)
        {
            case EmotionType.Blow:
                animName = CharacterCtrl.EMOJI_BLOW;
                break;
            case EmotionType.Burp:
                animName = CharacterCtrl.EMOJI_BURP;
                break;
            case EmotionType.Cool:
                animName = CharacterCtrl.EMOJI_COOL;
                break;
            case EmotionType.Cute:
                animName = CharacterCtrl.EMOJI_CUTE;
                break;
            case EmotionType.Demon:
                animName = CharacterCtrl.EMOJI_DEMON;
                break;
            case EmotionType.Drink:
                animName = CharacterCtrl.EMOJI_DRINK;
                break;
            case EmotionType.Drooling:
                animName = CharacterCtrl.EMOJI_DROOLING;
                break;
            case EmotionType.Freeze:
                animName = CharacterCtrl.EMOJI_FREEZE;
                break;
            case EmotionType.Fresh:
                animName = CharacterCtrl.EMOJI_FRESH;
                break;
            case EmotionType.Happy:
                animName = CharacterCtrl.EMOJI_HAPPY;
                break;
            case EmotionType.Licklips:
                animName = CharacterCtrl.EMOJI_LICKLIPS;
                break;
            case EmotionType.Rainbow:
                animName = CharacterCtrl.EMOJI_RAINBOW;
                break;
            case EmotionType.Relax:
                animName = CharacterCtrl.EMOJI_RELAX;
                break;
            case EmotionType.Shock:
                animName = CharacterCtrl.EMOJI_SHOCK;
                break;
            case EmotionType.Sour:
                animName = CharacterCtrl.EMOJI_SOUR;
                break;
            case EmotionType.Spicy:
                animName = CharacterCtrl.EMOJI_SPICY;
                break;
            case EmotionType.None:
            default:
                animName = CharacterCtrl.IDLE;
                break;
        }

        return animName;
    }

    public override bool IsSuitable()
    {
        return true;
    }
}
