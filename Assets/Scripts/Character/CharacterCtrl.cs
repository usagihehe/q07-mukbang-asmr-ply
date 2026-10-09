using Spine;
using Spine.Unity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterCtrl : MonoBehaviour
{
    [SerializeField]
    private SkeletonGraphic _skeleton;
    [SerializeField]
    private RectTransform _mPoint;
    public SkeletonGraphic Skeleton => _skeleton;
    public RectTransform mPoint => _mPoint;

    public TrackEntry PlayAnimByName(string animName, bool loop, Spine.AnimationState.TrackEntryDelegate callbackOnComplete = null)
    {
        TrackEntry trackfalse = _skeleton.AnimationState.SetAnimation(0, animName, loop);
        trackfalse.Complete += callbackOnComplete;
        return trackfalse;
    }

    // Polled instead of relying on Complete: an interrupted entry never fires Complete.
    public bool IsPlaying(TrackEntry entry)
    {
        return entry != null && _skeleton.AnimationState.GetCurrent(0) == entry && !entry.IsComplete;
    }

    private void OnEnable()
    {
        PlayAnimByName(IDLE, true, null);
    }

    public const string EMOJI_BLOW = "Emoji_Blow";
    public const string EMOJI_BURP = "Emoji_Burp";
    public const string EMOJI_COOL = "Emoji_Cool";
    public const string EMOJI_CUTE = "Emoji_Cute";
    public const string EMOJI_DEMON = "Emoji_Demon";
    public const string EMOJI_DRINK = "Emoji_Drink";
    public const string EMOJI_DROOLING = "Emoji_Drooling";
    public const string EMOJI_FREEZE = "Emoji_Freeze";
    public const string EMOJI_FRESH = "Emoji_Fresh";
    public const string EMOJI_HAPPY = "Emoji_Happy";
    public const string EMOJI_LICKLIPS = "Emoji_Licklips";
    public const string EMOJI_RAINBOW = "Emoji_Rainbow";
    public const string EMOJI_RELAX = "Emoji_Relax";
    public const string EMOJI_SHOCK = "Emoji_Shock";
    public const string EMOJI_SOUR = "Emoji_Sour";
    public const string EMOJI_SPICY = "Emoji_Spicy";
    public const string IDLE = "Idle";
    public const string MOUTH_DRINK = "Mouth_Drink";
    public const string MOUTH_EAT = "Mouth_Eat";
    public const string MOUTH_NOODLE = "Mouth_Noodle";
    public const string MOUTH_OPEN = "Mouth_Open";
    public const string TALK = "Talk";
    public const string EMOJI_SAD = "Emoji_Sad";


    public static readonly string[] EMOJIS = new string[]
    {
        EMOJI_BLOW,
        EMOJI_BURP,
        EMOJI_COOL,
        EMOJI_CUTE,
        EMOJI_DEMON,
        EMOJI_RELAX,
        EMOJI_SHOCK
    };

    public TrackEntry PlayAnimFlash(bool loop, Spine.AnimationState.TrackEntryDelegate callbackOnComplete = null)
    {
        // Get the current animation playing on track 0
        var currentAnim = _skeleton.AnimationState.GetCurrent(0);

        // Only proceed if the current animation is "Idle"
        if (currentAnim == null || currentAnim.Animation.Name != IDLE)
            return null;

        // Pick a random emoji animation from the list
        string animName = EMOJIS[Random.Range(0, EMOJIS.Length)];

        // Set the selected animation on track 0 with the specified loop setting
        TrackEntry trackEntry = _skeleton.AnimationState.SetAnimation(0, animName, loop);

        // Attach a callback to be called when the animation completes (if provided)
        if (callbackOnComplete != null)
            trackEntry.Complete += callbackOnComplete;

        // Return the TrackEntry for further use if needed
        return trackEntry;
    }
    public string GetAnimEmotion(EmotionType emotionType)
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

    public string GetAnimName(SoundType type)
    {
        string soundName = MOUTH_EAT;

        switch (type)
        {
            case SoundType.Drink:
                soundName = MOUTH_DRINK;
                break;
            case SoundType.Noodle:
                soundName = MOUTH_NOODLE;
                break;
            default:
                soundName = MOUTH_EAT;
                break;
        }

        return soundName;
    }

}



