namespace PLY33.Blindbox
{
    public class BlindboxIdleState : BlindboxState
    {
        private const string IdleAnim = "idle";

        public override string StateName => BlindboxStateMachine.Idle;

        public override void OnEnter()
        {
            Model.AnimationState.SetAnimation(0, IdleAnim, true);
        }

        public override string OnUpdate(float deltaTime)
        {
            return Machine.ConsumePlayRequest() ? BlindboxStateMachine.Act : null;
        }
    }
}
