using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetingState : PlayerBaseState
{


    private readonly int TargetingBlendTreeHash = Animator.StringToHash("TargetingBlendTree");
    private readonly int TargetingForwardHash = Animator.StringToHash("TargetingForward");
    private readonly int TargetingRightHash = Animator.StringToHash("TargetingRight");
    public PlayerTargetingState(PlayerStateMachine stateMachines) : base(stateMachines) { }

    private const float CrossFadeDuration = 0.1f;
    public override void Enter()
    {
        stateMachines.inputReader.TargetEvent += OnTarget;
        stateMachines.inputReader.DodgeEvent += OnDodge;
        stateMachines.inputReader.JumpEvent += OnJump;
        stateMachines.Animator.CrossFadeInFixedTime(TargetingBlendTreeHash, CrossFadeDuration);
    }
    public override void Tick(float deltaTime)
    {

        if (stateMachines.inputReader.IsAttacking)
        {
            stateMachines.SwitchState(new PlayerAttackingState(stateMachines, 0));
            return;
        }
        if (stateMachines.inputReader.IsBlocking)
        {
            stateMachines.SwitchState(new PlayerBlockingState(stateMachines));
            return;
        }
        if (stateMachines.Targeter.CurrentTarget == null)
        {
            stateMachines.SwitchState(new PlayerFreeLookState(stateMachines));
            return;
        }
        Vector3 movent = CalculateMovement(deltaTime);
        Move(movent * stateMachines.TargetingMovementSpeed, deltaTime);
        UpdateAnimator(deltaTime);
        FaceTarget();
    }
    public override void Exit()
    {
        stateMachines.inputReader.TargetEvent -= OnTarget;
        stateMachines.inputReader.DodgeEvent -= OnDodge;
        stateMachines.inputReader.JumpEvent -= OnJump;

    }
    private void OnTarget()
    {
        stateMachines.Targeter.Cancel();
        stateMachines.SwitchState(new PlayerFreeLookState(stateMachines));
    }
    private Vector3 CalculateMovement(float deltaTime)
    {
        Vector3 movent = new Vector3();
        movent += stateMachines.transform.right * stateMachines.inputReader.MovementValue.x;
        movent += stateMachines.transform.forward * stateMachines.inputReader.MovementValue.y;

        return movent;
    }
    private void UpdateAnimator(float deltaTime)
    {
        if (stateMachines.inputReader.MovementValue.y == 0)
        {
            stateMachines.Animator.SetFloat(TargetingForwardHash, 0, 0.1f, deltaTime);
        }
        else
        {
            float value = stateMachines.inputReader.MovementValue.y > 0 ? 1f : -1f;
            stateMachines.Animator.SetFloat(TargetingForwardHash, value, 0.1f, deltaTime);
        }

        if (stateMachines.inputReader.MovementValue.x == 0)
        {
            stateMachines.Animator.SetFloat(TargetingRightHash, 0, 0.1f, deltaTime);
        }
        else
        {
            float value = stateMachines.inputReader.MovementValue.x > 0 ? 1f : -1f;
            stateMachines.Animator.SetFloat(TargetingRightHash, value, 0.1f, deltaTime);
        }
    }
    private void OnDodge()
    {
        if (stateMachines.inputReader.MovementValue == Vector2.zero) { return; }
        stateMachines.SwitchState(new PlayerDodgingState(stateMachines, stateMachines.inputReader.MovementValue));
    }
    private void OnJump()
    {
        stateMachines.SwitchState(new PlayerJumpingState(stateMachines));
    }
}
