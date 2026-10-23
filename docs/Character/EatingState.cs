using Capyboba.Mukbang;
using Spine;
using UnityEngine;

public class EatingState : CharBaseState, IBiteAnimator
{
    private TrackEntry _entry;
    private BiteStatus _status;

    public override void EnterState()
    {
        _entry = null;
        _status = BiteStatus.Completed;
        _stateMachine.Brain.BeginEating();
    }

    public override string Execute(float dt)
    {
        // Another script replaced our animation, so its Complete callback will never run.
        if (_status == BiteStatus.Playing && !_character.IsCurrent(_entry)) _status = BiteStatus.Interrupted;
        if (_status != BiteStatus.Playing) AudioManager.Instance.StopSoundEffect();

        EaterState next = _stateMachine.Brain.TickEating(_stateMachine.Mouth, _status);
        if (next != EaterState.Eating) AudioManager.Instance.StopSoundEffect();
        return StateMachine.ToKey(next);
    }

    public override void ExitState()
    {
    }

    public override bool IsSuitable()
    {
        return true;
    }

    public void PlayBite(IEdible item)
    {
        _status = BiteStatus.Playing;
        AudioClip sound = GameManager.Instance.CharSound.GetEatSound(item.EatSound);
        if (sound != null) AudioManager.Instance.PlaySoundEffect(sound);
        _entry = _character.PlayAnimByName(GetAnimName(item.EatSound), false, _ => _status = BiteStatus.Completed);
    }

    private string GetAnimName(SoundType type)
    {
        switch (type)
        {
            case SoundType.Drink:
                return CharacterCtrl.MOUTH_DRINK;
            case SoundType.Noodle:
                return CharacterCtrl.MOUTH_NOODLE;
            default:
                return CharacterCtrl.MOUTH_EAT;
        }
    }
}
