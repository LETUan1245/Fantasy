using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockingState : PlayerBaseState
{
    private readonly int BlockHash = Animator.StringToHash("Block");


    private const float CrossFadeDuration = 0.1f;
    public PlayerBlockingState(PlayerStateMachine stateMachines) : base(stateMachines)
    {
    }

    public override void Enter()
    {
        stateMachines.Health.SetInvunerable(true);
        stateMachines.Animator.CrossFadeInFixedTime(BlockHash, CrossFadeDuration);
    }
    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        if (!stateMachines.inputReader.IsBlocking)
        {
            stateMachines.SwitchState(new PlayerTargetingState(stateMachines));
            return;
        }
        if (stateMachines.Targeter.CurrentTarget == null)
        {
            stateMachines.SwitchState(new PlayerFreeLookState(stateMachines));
            return;
        }
    }
    public override void Exit()
    {
        stateMachines.Health.SetInvunerable(false);
    }




}
