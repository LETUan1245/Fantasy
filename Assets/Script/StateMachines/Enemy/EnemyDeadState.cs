using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadState : EnemyBaseState
{
    public EnemyDeadState(EnemyStateMachine stateMachines) : base(stateMachines)
    {
    }

    public override void Enter()
    {
        stateMachines.Ragdoll.ToggleRagdoll(true);
        stateMachines.Weapon.gameObject.SetActive(false);
        GameObject.Destroy(stateMachines.Target);
    }
    public override void Tick(float deltaTime)
    {

    }

    public override void Exit()
    {

    }



}
