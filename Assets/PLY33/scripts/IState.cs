namespace PLY33.Blindbox
{
    public interface IState
    {
        string StateName { get; }
        void OnEnter();
        /// <summary>Returns the next state's name, or null to stay.</summary>
        string OnUpdate(float deltaTime);
        void OnExit();
        bool IsSuitable();
    }
}
