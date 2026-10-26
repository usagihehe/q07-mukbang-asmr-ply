using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PayMachineState : MonoBehaviour
{
    public const string Idle = "idle";
    public const string WaitScan = "wait-scan";
    public const string Scan = "scan";
    public const string Done = "done";

    [SerializeField]
    private float _distanceScan;
    [SerializeField]
    protected string _currentState;
    [SerializeField]
    private PayMachineCtrl _payMachine;
    [SerializeField]
    private GameObject _handObj;
    protected Dictionary<string, CharBaseState> _states;
    public ScanItem SelectItem;
    public float DistanceScan => _distanceScan;
    public string CurrentState => _currentState;

    private void Awake()
    {
        _states = new Dictionary<string, CharBaseState>()
        {
            {Idle, GetComponent<PayIdleState>() },
            {WaitScan, GetComponent<WaitScanState>() },
            {Scan, GetComponent<ScanState>() },
            {Done, GetComponent<PayDoneState>() },
        };
    }

    protected virtual void OnEnable()
    {
        Observer.Instance.AddObserver(ObserverTopic.OnSelectItem, OnSelectItem);
        Observer.Instance.AddObserver(ObserverTopic.OnDropItem, OnDropItem);
        _currentState = Idle;
        if (_states.ContainsKey(_currentState))
        {
            _states[_currentState].EnterState();
        }
    }

    protected virtual void OnDisable()
    {
        Observer.Instance.RemoveObserver(ObserverTopic.OnSelectItem, OnSelectItem);
        Observer.Instance.RemoveObserver(ObserverTopic.OnDropItem, OnDropItem);
        SelectItem = null;
        if (_states.ContainsKey(_currentState))
        {
            _states[_currentState].ExitState();
        }
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

    // Overlay canvas positions are screen pixels; divide like the eat distance so _distanceScan is in 1080-wide canvas units.
    public float GetDistance()
    {
        float canvasScale = GetComponentInParent<Canvas>().rootCanvas.scaleFactor;
        return Vector3.Distance(_payMachine.Scan.transform.position, SelectItem.transform.position) / canvasScale;
    }

    public CharBaseState GetState(string stateName)
    {
        return _states.ContainsKey(stateName) ? _states[stateName] : null;
    }

    // Food items share these topics; only ScanItems belong to the pay machine.
    private void OnSelectItem(object data)
    {
        if (data is ScanItem item) SelectItem = item;
    }

    private void OnDropItem(object data)
    {
        if (data is ScanItem item && item == SelectItem) SelectItem = null;
    }
}
