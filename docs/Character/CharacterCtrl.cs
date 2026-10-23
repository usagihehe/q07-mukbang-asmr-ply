using Spine;
using Spine.Unity;
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
        TrackEntry trackEntry = _skeleton.AnimationState.SetAnimation(0, animName, loop);
        AddCompleteCallback(trackEntry, callbackOnComplete);
        return trackEntry;
    }

    public bool IsCurrent(TrackEntry entry)
    {
        return entry != null && _skeleton.AnimationState.GetCurrent(0) == entry;
    }

    // Spine still fires Complete on a replaced entry while it mixes out (defaultMix 0.2s),
    // so a stale callback could override the animation that replaced it.
    private void AddCompleteCallback(TrackEntry entry, Spine.AnimationState.TrackEntryDelegate callback)
    {
        if (callback == null) return;
        entry.Complete += e =>
        {
            if (IsCurrent(e)) callback(e);
        };
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
    public const string EMOJI_SAD = "Emoji_Sad";
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
        AddCompleteCallback(trackEntry, callbackOnComplete);

        // Return the TrackEntry for further use if needed
        return trackEntry;
    }

}
