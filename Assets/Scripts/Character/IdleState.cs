using Capyboba.Mukbang;
using UnityEngine;

public class IdleState : CharBaseState
{
    [SerializeField]
    private float _timeHungry = 5;
    private float _leanId;

    public override void EnterState()
    {
        // StateMachine.OnEnable can call this before our own Awake has run.
        if (_character == null || _stateMachine == null) base.Awake();
        _leanId = 0;
        _character.PlayAnimByName(CharacterCtrl.IDLE, true);
    }

    public override string Execute(float dt)
    {
        if (_stateMachine.Brain.TickIdle(_stateMachine.Mouth) == EaterState.BeforeEat) return StateMachine.BeforeEatState;
        _leanId += dt;
        if (_leanId >= _timeHungry)
        {
            _leanId = 0;
            _character.PlayAnimByName(CharacterCtrl.EMOJI_LICKLIPS, false, delegate
            {
                _character.PlayAnimByName(CharacterCtrl.IDLE, true);
            });
        }
        return StateMachine.IdleState;
    }

    public override void ExitState()
    {
        _leanId = 0;
    }

    public override bool IsSuitable()
    {
        return true;
    }
}
