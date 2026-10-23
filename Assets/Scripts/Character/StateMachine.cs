using Capyboba.Mukbang;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public const string IdleState = "idle";
    public const string EatingState = "eating";
    public const string BeforeEatState = "before-eat";
    public const string AfterEatState = "after-eat";

    [SerializeField]
    private float _distanceBeforeEat;

    [SerializeField]
    private float _distanceEating;

    [SerializeField]
    private EaterGroup _group = EaterGroup.Main;

    [SerializeField]
    protected string _currentState;

    protected Dictionary<string, CharBaseState> _states;

    private CharacterCtrl _character;

    private HeldItemTracker _tracker;

    public EaterBrain Brain { get; private set; }

    public Vector3 Mouth => _character.mPoint.position;

    public static string ToKey(EaterState state)
    {
        switch (state)
        {
            case EaterState.BeforeEat: return BeforeEatState;
            case EaterState.Eating: return EatingState;
            case EaterState.AfterEat: return AfterEatState;
            default: return IdleState;
        }
    }

    protected virtual void OnEnable()
    {
        Observer.Instance.AddObserver(ObserverTopic.OnSelectItem, OnSelectItem);
        Observer.Instance.AddObserver(ObserverTopic.OnDropItem, OnDropItem);
        _currentState = IdleState;
        if (_states == null)
        {
            _states = new Dictionary<string, CharBaseState>
            {
                { IdleState, GetComponent<IdleState>() },
                { AfterEatState, GetComponent<AfterEatState>() },
                { EatingState, GetComponent<EatingState>() },
                { BeforeEatState, GetComponent<BeforeEatState>() }
            };
            _character = GetComponent<CharacterCtrl>();
        }
        if (_tracker == null)
        {
            _tracker = new HeldItemTracker(e => e.Group == _group);
            Brain = new EaterBrain(_tracker, GetComponent<EatingState>(), _distanceBeforeEat, _distanceEating);
        }
        _tracker.Clear();
        Brain.Reset();
        _states[_currentState].EnterState();
    }

    protected virtual void OnDisable()
    {
        Observer.Instance.RemoveObserver(ObserverTopic.OnSelectItem, OnSelectItem);
        Observer.Instance.RemoveObserver(ObserverTopic.OnDropItem, OnDropItem);
        _tracker.Clear();
    }

    protected virtual void Update()
    {
        if (_states.ContainsKey(_currentState))
        {
            string nextState = _states[_currentState].Execute(Time.deltaTime);
            if (!string.IsNullOrEmpty(nextState) && _states.ContainsKey(nextState) && nextState != _currentState)
            {
                _states[_currentState].ExitState();
                _currentState = nextState;
                if (_states[_currentState].IsSuitable()) _states[_currentState].EnterState();
            }
        }
    }

    private void OnSelectItem(object data)
    {
        _tracker.OnSelect(data);
    }

    private void OnDropItem(object data)
    {
        _tracker.OnDrop(data);
    }
}
