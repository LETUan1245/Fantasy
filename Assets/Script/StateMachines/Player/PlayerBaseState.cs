using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState : State
{
    protected PlayerStateMachine stateMachines;
    public PlayerBaseState(PlayerStateMachine stateMachines)
    {
        this.stateMachines = stateMachines;
    }
    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }
    protected void Move(Vector3 motion, float deltaTime)
    {
        stateMachines.Controller.Move((motion + stateMachines.ForceReceiver.Movement) * deltaTime);
    }
    protected void FaceTarget()
    {
        if (stateMachines.Targeter.CurrentTarget == null) { return; }

        Vector3 lookPos = stateMachines.Targeter.CurrentTarget.transform.position - stateMachines.transform.position;
        lookPos.y = 0f;

        stateMachines.transform.rotation = Quaternion.LookRotation(lookPos);
    }
    protected void ReturnToLocomotion()
    {
        if (stateMachines.Targeter.CurrentTarget != null)
        {
            stateMachines.SwitchState(new PlayerTargetingState(stateMachines));
        }
        else
        {
            stateMachines.SwitchState(new PlayerFreeLookState(stateMachines));
        }
    }
}
