using Usaki;

public class WaitScanState : CharBaseState
{
    private PayMachineCtrl _payMachine;

    private PayMachineState _payMachineState;

    protected override void Awake()
    {
        _stateName = PayMachineState.WaitScan;
        _payMachine = GetComponent<PayMachineCtrl>();
        _payMachineState = GetComponent<PayMachineState>();
        _nextStates = new string[]
        {
              PayMachineState.Scan
        };
    }

    public override void EnterState()
    {
        _payMachine.Scan.SetState(IMGState.ON);
        _payMachine.ScaneEffect.gameObject.SetActive(true);
        _payMachine.DoneBtn.gameObject.SetActive(false);
    }

    public override string Execute(float dt)
    {
        if (_payMachineState.SelectItem != null
            && _payMachineState.GetDistance() <= _payMachineState.DistanceScan
            && !_payMachineState.SelectItem.IsScan)
            return PayMachineState.Scan;
        else
            if (_payMachineState.SelectItem == null) return PayMachineState.Idle;
        return PayMachineState.WaitScan;
    }

    public override void ExitState()
    {
        _payMachine.Scan.SetState(IMGState.OFF);
        _payMachine.ScaneEffect.gameObject.SetActive(false);
    }

    public override bool IsSuitable()
    {
        return _payMachineState.SelectItem != null;
    }
}
