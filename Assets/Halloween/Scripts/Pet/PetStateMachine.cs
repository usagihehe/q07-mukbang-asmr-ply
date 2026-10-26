using Capyboba.Mukbang;
using SLunch.Livestream;
using System.Collections.Generic;
using UnityEngine;

namespace Pet
{
    public class PetStateMachine : MonoBehaviour
    {
        public const string IdleState = "idle";
        public const string EatingState = "eating";
        public const string AfterEatState = "after";
        public const string DragState = "drag";
        [SerializeField] private float _distanceEating;
        [SerializeField] protected string _currentState;
        protected Dictionary<string, PetBaseState> _states;
        private PetCtrl _character;
        private HeldItemTracker _tracker;
        public IEdible Held => _tracker.Held;
        public HeldItemTracker Tracker => _tracker;
        public float DistanceEating => _distanceEating;

        /// <summary>Tên state hiện tại (để input / drag kiểm tra).</summary>
        public string CurrentState => _currentState;

        protected virtual void Awake()
        {
            BuildStatesDictionary();
            _character = GetComponent<PetCtrl>();
        }

        protected virtual void Start()
        {
            BuildStatesDictionary();
            _character = GetComponent<PetCtrl>();
        }

        protected virtual void OnEnable()
        {
            Observer.Instance.AddObserver(ObserverTopic.OnSelectItem, OnSelectItem);
            Observer.Instance.AddObserver(ObserverTopic.OnDropItem, OnDropItem);
            if (_tracker == null)
                _tracker = new HeldItemTracker(IsPetFood);
            _tracker.Clear();
            _currentState = IdleState;
            if (_states == null)
                BuildStatesDictionary();
            if (_states.ContainsKey(_currentState))
            {
                _states[_currentState].EnterState();
            }
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

        public PetBaseState GetState(string stateName)
        {
            if (_states == null)
                return null;
            return _states.ContainsKey(stateName) ? _states[stateName] : null;
        }

        public float Distance()
        {
            return Vector3.Distance(_character.mPoint.transform.position, Held.EatPoint) / Held.CanvasScale;
        }

        private void OnSelectItem(object data)
        {
            _tracker.OnSelect(data);
        }

        private void OnDropItem(object data)
        {
            _tracker.OnDrop(data);
        }

        /// <summary>Chuyển state ngay (dùng cho input kéo, không đi qua Execute). Chỉ cho phép vào Drag từ Idle.</summary>
        public bool TrySetState(string nextState)
        {
            if (_states == null)
                BuildStatesDictionary();
            if (string.IsNullOrEmpty(nextState) || !_states.ContainsKey(nextState) || nextState == _currentState)
                return false;
            if (_states[nextState] == null)
                return false;
            if (_states[_currentState] == null)
                return false;
            if (nextState == DragState && _currentState != IdleState)
                return false;

            _states[_currentState].ExitState();
            _currentState = nextState;
            if (_states[_currentState].IsSuitable())
                _states[_currentState].EnterState();
            return true;
        }

        /// <summary>The pet never drinks: drinks of every mode are not held for it, so it stays idle.</summary>
        private static bool IsPetFood(IEdible e)
        {
            if (e.Style == EatStyle.Sip) return false;
            if (e is SupermarketItemMukbang smk) return !SupermarketItemData.Instance.IsDrinkItem(smk.Item);
            return e is BaseSlotMukbang;
        }

        private void BuildStatesDictionary()
        {
            _states = new Dictionary<string, PetBaseState>
            {
                { IdleState, GetComponent<PetIdleState>() },
                { EatingState, GetComponent<PetEatState>() },
                { AfterEatState, GetComponent<PetAfterEatState>() },
                { DragState, GetComponent<PetDragState>() },
            };
        }
    }
}
