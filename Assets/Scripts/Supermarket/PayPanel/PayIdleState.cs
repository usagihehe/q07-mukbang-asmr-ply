using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Usaki;

public class PayIdleState : CharBaseState
{
    private PayMachineCtrl _payMachine;
    private PayMachineState _payMachineState;

    protected override void Awake()
    {
        _stateName = PayMachineState.Idle;
        _payMachineState = GetComponent<PayMachineState>();
        _payMachine = GetComponent<PayMachineCtrl>();
        _nextStates = new string[]
        {
            PayMachineState.WaitScan
        };
    }

    public override void EnterState()
    {
        if (_payMachineState == null) _payMachineState = GetComponent<PayMachineState>();
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
