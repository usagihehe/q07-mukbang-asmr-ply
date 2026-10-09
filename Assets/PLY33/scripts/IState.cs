namespace PLY33.Blindbox
{
    public interface IState
    {
        string StateName { get; }
        /// <summary>States this one may hand over to in the normal flow.</summary>
        string[] NextStates { get; }
        /// <summary>States this one may also jump to, outside the normal flow.</summary>
        string[] CrossStates { get; }
        void OnEnter();
        /// <summary>Returns the next state's name, or null to stay.</summary>
        string OnUpdate(float deltaTime);
        void OnExit();
        bool IsSuitable();
    }
}
