using Spine.Unity;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class PlayAnimClick : MonoBehaviour
{
    [SerializeField]
    private SkeletonGraphic _skeleton;

    [SerializeField]
    private EmotionType[] _animList;

    [SerializeField]
    private bool _waitOnDone;

    private bool _canPlayAnim = true;
    private int _lastIndex = -1;

    private void Awake()
    {
        if (_skeleton == null)
        {
            Debug.LogError("SkeletonGraphic is not assigned!");
        }
        if (_animList == null || _animList.Length == 0)
        {
            Debug.LogWarning("Animation list is empty!");
        }
        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (_canPlayAnim) { PlayRandomAnimation(); }
        });
    }

    private EmotionType GetRandomAnimation()
    {
        if (_animList == null || _animList.Length == 0)
        {
            return EmotionType.None;
        }
        int newIndex;
        do
        {
            newIndex = Random.Range(0, _animList.Length);
        } while (newIndex == _lastIndex && _animList.Length > 1);

        _lastIndex = newIndex;
        return _animList[newIndex];
    }

    private void PlayRandomAnimation()
    {
        EmotionType emotionType = GetRandomAnimation();
        string anim = GetAnimEmotion(emotionType);
        _canPlayAnim = false;
        _skeleton.AnimationState.ClearTracks();
        _skeleton.Skeleton.SetToSetupPose();
        AudioManager.Instance.PlaySoundEffect(GetSound(emotionType));
        _skeleton.AnimationState.SetAnimation(0, anim, false).Complete += delegate
        {
            _skeleton.AnimationState.SetAnimation(0, CharacterCtrl.IDLE, true);
            _canPlayAnim = true;
            AudioManager.Instance.StopSoundEffect();
        };
    }


    private AudioClip GetSound(EmotionType type)
    {
        return GameManager.Instance.CharSound.GetEmotionSound(type);
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
}
