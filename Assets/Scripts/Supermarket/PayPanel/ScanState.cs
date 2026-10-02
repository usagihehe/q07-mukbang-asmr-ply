using DG.Tweening;
using UnityEngine;
using Usaki;

public class ScanState : CharBaseState
{
    [SerializeField] private float _scaleVal;
    [SerializeField] private float _timeScale;
    [SerializeField] private float _timeMove;
    [SerializeField] private Transform _toPos;
    [SerializeField] private AudioClip _scanSound;
    [SerializeField] private Ease _easeMove;
    private PayMachineCtrl _payMachine;
    private PayMachineState _payMachineState;
    private bool _isExecute;
    private ScanItem _item;

    protected override void Awake()
    {
        _stateName = PayMachineState.Scan;
        _payMachine = GetComponent<PayMachineCtrl>();
        _payMachineState = GetComponent<PayMachineState>();
        _nextStates = new string[]
        {
              PayMachineState.Idle,
              PayMachineState.Done
        };
    }

    public override void EnterState()
    {
        _isExecute = false;
        _item = _payMachineState.SelectItem;
    }

    public override string Execute(float dt)
    {
        if (_isExecute)
        {
            return PayMachineState.Scan;
        }
        if (_item.IsScan)
        {
            if (_payMachine.IsScanAll())
            {
                return PayMachineState.Done;
            }
            return PayMachineState.Idle;
        }

        _isExecute = true;
        AudioManager.Instance.PlaySoundEffect(_scanSound);
        _payMachine.CurScreen = PayScreen.Bill;
        _payMachine.Scan.SetState(IMGState.OFF);
        _payMachine.ScaneEffect.gameObject.SetActive(false);
        _item.IsScan = true;
        _item.BlockControl = true;
        _item.transform.DOScale(_scaleVal, _timeScale)
            .SetEase(_easeMove)
            .SetLoops(2, LoopType.Yoyo)
            .OnComplete(() =>
            {
                _item.transform.DOMove(_toPos.position, _timeMove)
                .SetEase(_easeMove)
                .OnComplete(() => _isExecute = false);
            });
        _payMachine.PrintItem(_item.SO);

        return PayMachineState.Scan;
    }

    public override void ExitState()
    {
        _item = null;
        _isExecute = false;
    }

    public override bool IsSuitable()
    {
        return true;
    }
}
