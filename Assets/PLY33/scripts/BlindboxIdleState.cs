namespace PLY33.Blindbox
{
    /// <summary>Machine waits for the catcher button; the only state where it is pressable.</summary>
    public class BlindboxIdleState : BlindboxState
    {
        public override void OnEnter()
        {
            // The idle hint queues idle after its one-shot act; don't cut it short. A held gacha frame has nothing queued.
            if (_Model.AnimationState.GetCurrent(0)?.Next == null)
            {
                _Model.AnimationState.SetAnimation(0, _StateMachine.Idle, true);
            }
            _StateMachine.CatcherBtn.interactable = true;
        }

        // Act refuses to start until the catcher has been pressed (see BlindboxActState.IsSuitable).
        public override string OnUpdate(float deltaTime)
        {
            return _StateMachine.Act;
        }

        public override bool IsSuitable()
        {
            return true;
        }
    }
}
