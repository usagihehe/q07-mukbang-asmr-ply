using UnityEngine;

public abstract class CharBaseState : MonoBehaviour
{
    protected CharacterCtrl _character;

    protected StateMachine _stateMachine;

    protected virtual void Awake()
    {
        _character = GetComponent<CharacterCtrl>();
        _stateMachine = GetComponent<StateMachine>();
    }

    public abstract void EnterState();

    public abstract string Execute(float dt);

    public abstract void ExitState();

    public abstract bool IsSuitable();
}
