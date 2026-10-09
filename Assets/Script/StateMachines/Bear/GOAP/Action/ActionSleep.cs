using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSleep : GAction
{
    private BearStateMachine fsm;

    private void Start()
    {
        fsm = GetComponent<BearStateMachine>();
    }

    public override bool PrePerform()
    {
        target = this.gameObject;

        if (fsm != null) fsm.SwitchState(new BearSleepState(fsm));
        return true;
    }

    public override bool PostPerform()
    {
        if (fsm != null) fsm.SwitchState(new BearIdleState(fsm));
        GetComponent<BearAgent>().beliefs.RemoveState("hasEaten");
        return true;
    }
}
