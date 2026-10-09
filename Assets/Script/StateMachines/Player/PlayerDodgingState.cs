using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgingState : PlayerBaseState
{
    private readonly int DodgeBlendTreeHash = Animator.StringToHash("DodgeBlendTree");

    private readonly int DodgeForwardHash = Animator.StringToHash("DodgeForward");

    private readonly int DodgeRightHash = Animator.StringToHash("DodgeRight");
    private Vector3 dodgingDirectionInput;
    private float remainingDodgeTime;
    private const float CrossFadeDuration = 0.1f;
    public PlayerDodgingState(PlayerStateMachine stateMachines, Vector3 dodgingDirectionInput) : base(stateMachines)
    {
        this.dodgingDirectionInput = dodgingDirectionInput;
    }
    public override void Tick(float deltaTime)
    {
        Vector3 movent = new Vector3();
        movent += stateMachines.transform.right * dodgingDirectionInput.x * stateMachines.DodgeLength / stateMachines.DodgeDuration;
        movent += stateMachines.transform.forward * dodgingDirectionInput.y * stateMachines.DodgeLength / stateMachines.DodgeDuration;

        Move(movent, deltaTime);
        FaceTarget();

        remainingDodgeTime -= deltaTime;
        if (remainingDodgeTime <= 0f)
        {
            stateMachines.SwitchState(new PlayerTargetingState(stateMachines));
        }
    }
    public override void Enter()
    {
        remainingDodgeTime = stateMachines.DodgeDuration;

        stateMachines.Animator.SetFloat(DodgeForwardHash, dodgingDirectionInput.y);
        stateMachines.Animator.SetFloat(DodgeRightHash, dodgingDirectionInput.x);
        stateMachines.Animator.CrossFadeInFixedTime(DodgeBlendTreeHash, CrossFadeDuration);

        stateMachines.Health.SetInvunerable(true);
    }

    public override void Exit()
    {
        stateMachines.Health.SetInvunerable(false);
    }



}
