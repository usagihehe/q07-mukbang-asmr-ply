using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayIdleState : CharBaseState
{
    private PayMachineCtrl _payMachine;
    private PayMachineState _payMachineState;

    protected override void Awake()
    {
        _payMachineState = GetComponent<PayMachineState>();
        _payMachine = GetComponent<PayMachineCtrl>();
    }

    public override void EnterState()
    {
        if (_payMachineState == null) _payMachineState = GetComponent<PayMachineState>();
        //_payMachineState.SelectItem = null;
        if (_payMachine == null) _payMachine = GetComponent<PayMachineCtrl>();
        _payMachine.Scan.SetState(IMGState.OFF);
        _payMachine.ScaneEffect.gameObject.SetActive(false);
    }

    public override string Execute(float dt)
    {
        if (_payMachineState.SelectItem != null) return PayMachineState.WaitScan;
        else return PayMachineState.Idle;
    }

    public override void ExitState()
    {
    }

    public override bool IsSuitable()
    {
        return true;
    }
}
