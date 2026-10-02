using System.Collections.Generic;
using UnityEngine;
namespace Usaki
{
    public class StateMachine : MonoBehaviour
    {
        public const string IdleState = "idle";
        public const string EatingState = "eating";
        public const string BeforeEatState = "before-eat";
        public const string Hungry1 = "hungry1";
        public const string Hungry = "hungry";
        public const string AfterEatState = "after-eat";
        public const string SelfieState = "selfie";
        public const string StunState = "stun";

        [SerializeField]
        private float _distanceBeforeEat;

        [SerializeField]
        private float _distanceEating;

        [SerializeField]
        protected string _currentState;

        protected Dictionary<string, CharBaseState> _states;

        private CharacterCtrl _character;

        public bool IsSelectItem;

        public ItemMukbang MukbangItem { get; set; }

        public float DistanceBeforeEat => _distanceBeforeEat;

        public float DistanceEating => _distanceEating;

        protected virtual void Awake()
        {

        }

        protected virtual void Start()
        {
            _states = new Dictionary<string, CharBaseState>
        {
            { IdleState, GetComponent<IdleState>() },
            { AfterEatState, GetComponent<AfterEatState>() },
            { EatingState, GetComponent<EatingState>() },
            { BeforeEatState, GetComponent<BeforeEatState>() },
            { SelfieState, GetComponent<SelfieState>() }
        };
            _character = GetComponent<CharacterCtrl>();
        }

        protected virtual void OnEnable()
        {
            Observer.Instance.AddObserver(ObserverTopic.OnSelectItem, OnSelectItem);
            Observer.Instance.AddObserver(ObserverTopic.OnDropItem, OnDropItem);
            IsSelectItem = false;
            _currentState = IdleState;
            if (_states == null)
            {
                _states = new Dictionary<string, CharBaseState>
            {
                { IdleState, GetComponent<IdleState>() },
                { AfterEatState, GetComponent<AfterEatState>() },
                { EatingState, GetComponent<EatingState>() },
                { BeforeEatState, GetComponent<BeforeEatState>() },
                { SelfieState, GetComponent<SelfieState>() }
            };
            }
            if (_states.ContainsKey(_currentState))
            {
                _states[_currentState].EnterState();
            }
        }

        protected virtual void OnDisable()
        {
            Observer.Instance.RemoveObserver(ObserverTopic.OnSelectItem, OnSelectItem);
            Observer.Instance.RemoveObserver(ObserverTopic.OnDropItem, OnDropItem);
            MukbangItem = null;
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

        public CharBaseState GetState(string stateName)
        {
            return _states.ContainsKey(stateName) ? _states[stateName] : null;
        }

        public float Distance()
        {
            if (SupermarketItemData.Instance.IsDrinkItem((MukbangItem as SupermarketItemMukbang).Item))
            {
                return Vector3.Distance(_character.mPoint.transform.position, MukbangItem.Tool.transform.position + Vector3.up * 50f);
            }

            return Vector3.Distance(_character.mPoint.transform.position, MukbangItem.Tool.transform.position);
        }

        private void OnSelectItem(object data)
        {
            IsSelectItem = true;
            if (data is SupermarketItemMukbang supermarketItem)
            {
                MukbangItem = supermarketItem;
            }
        }

        private void OnDropItem(object data)
        {
            IsSelectItem = false;
        }
    }
}
