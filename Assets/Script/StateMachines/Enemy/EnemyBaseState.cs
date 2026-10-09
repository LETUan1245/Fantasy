using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseState : State
{
    protected EnemyStateMachine stateMachines;
    public EnemyBaseState(EnemyStateMachine stateMachines)
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
        stateMachines.Agent.nextPosition = stateMachines.transform.position;
    }
    protected void FacePlayer()
    {
        if (stateMachines.Player == null)
        {
            return;
        }
        Vector3 lookPos = stateMachines.Player.transform.position -
            stateMachines.transform.position;
        lookPos.y = 0f;
        stateMachines.transform.rotation = Quaternion.LookRotation(lookPos);
    }
    protected bool IsInChaseRange()
    {
        if (stateMachines.Player.IsDead) { return false; }

        float playerDistanceSqr = (stateMachines.Player.transform.position - stateMachines.transform.position).sqrMagnitude;

        return playerDistanceSqr <= stateMachines.PlayerChasingRange * stateMachines.PlayerChasingRange;

    }
}
