using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : PlayerBaseState
{
    public PlayerDeadState(PlayerStateMachine stateMachines) : base(stateMachines)
    {
    }

    public override void Enter()
    {
        stateMachines.Ragdoll.ToggleRagdoll(true);
        stateMachines.weapon.gameObject.SetActive(false);
    }
    public override void Tick(float deltaTime)
    {

    }

    public override void Exit()
    {

    }



}
