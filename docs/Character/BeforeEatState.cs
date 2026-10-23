using Capyboba.Mukbang;

public class BeforeEatState : CharBaseState
{
    public override void EnterState()
    {
        if (GameManager.Instance.CharSound.GetEatSound(SoundType.Open) != null) AudioManager.Instance.PlaySoundEffect(GameManager.Instance.CharSound.GetEatSound(SoundType.Open));
        _character.PlayAnimByName(CharacterCtrl.MOUTH_OPEN, true);
    }

    public override string Execute(float dt)
    {
        EaterState next = _stateMachine.Brain.TickBeforeEat(_stateMachine.Mouth);
        // Leaving the open mouth (to eat, or the item was released / moved away) ends its sound.
        if (next != EaterState.BeforeEat) AudioManager.Instance.StopSoundEffect();
        return StateMachine.ToKey(next);
    }

    public override void ExitState()
    {
    }

    public override bool IsSuitable()
    {
        return true;
    }
}
