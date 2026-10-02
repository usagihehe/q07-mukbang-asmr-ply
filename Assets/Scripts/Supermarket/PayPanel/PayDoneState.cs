using UnityEngine;
using Usaki;

public class PayDoneState : CharBaseState
{
    private PayMachineCtrl _payMachine;

    protected override void Awake()
    {
        _stateName = PayMachineState.Done;
        _payMachine = GetComponent<PayMachineCtrl>();
    }

    public override void EnterState()
    {
        _payMachine.DoneBtn.gameObject.SetActive(true);
    }

    public override string Execute(float dt)
    {
        return PayMachineState.Done;
    }

    public override void ExitState()
    {
    }

    public override bool IsSuitable()
    {
        return true;
    }
}
