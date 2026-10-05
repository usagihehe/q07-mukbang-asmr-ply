using System.Collections.Generic;
using UnityEngine;

namespace PLY33.Blindbox
{
    /// <summary>Runs the IState components on this GameObject, keyed by state name.</summary>
    public abstract class BaseStateMachine : MonoBehaviour
    {
        private readonly Dictionary<string, IState> _states = new Dictionary<string, IState>();
        private IState _currentState;

        public string CurrentStateName => _currentState?.StateName;

        protected abstract string InitialState { get; }

        protected virtual void Awake()
        {
            foreach (IState state in GetComponents<IState>())
            {
                _states[state.StateName] = state;
            }
        }

        protected virtual void OnEnable()
        {
            ChangeState(InitialState);
        }

        protected virtual void OnDisable()
        {
            _currentState?.OnExit();
            _currentState = null;
        }

        private void Update()
        {
            if (_currentState == null) return;
            string nextStateName = _currentState.OnUpdate(Time.deltaTime);
            if (string.IsNullOrEmpty(nextStateName) || nextStateName == _currentState.StateName) return;
            if (!_states.TryGetValue(nextStateName, out IState nextState) || !nextState.IsSuitable()) return;
            ChangeState(nextStateName);
        }

        private void ChangeState(string stateName)
        {
            if (!_states.TryGetValue(stateName, out IState nextState))
            {
                Debug.LogError($"{name}: state '{stateName}' not found");
                return;
            }
            _currentState?.OnExit();
            _currentState = nextState;
            _currentState.OnEnter();
        }
    }
}
