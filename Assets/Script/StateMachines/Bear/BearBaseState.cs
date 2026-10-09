using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BearBaseState : State
{
    protected BearStateMachine stateMachine;
    public BearBaseState(BearStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }
    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }
    protected void Move(Vector3 motion, float deltaTime)
    {
        stateMachine.Controller.Move((motion + stateMachine.ForceReceiver.Movement) * deltaTime);
    }
}
