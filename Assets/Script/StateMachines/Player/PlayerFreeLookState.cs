using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFreeLookState : PlayerBaseState
{
    private bool shouldFade;

    private readonly int FreeLookSpeedHash = Animator.StringToHash("FreeLook"); //parameters
    private readonly int FreeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree"); // Blend Tree mau cam

    private const float AnimatorDampTime = 0.1f;
    private const float CrossFadeDuration = 0.1f;


    public PlayerFreeLookState(PlayerStateMachine stateMachines, bool shouldFade = true) : base(stateMachines)
    {
        this.shouldFade = shouldFade;
    }
    public override void Enter()
    {
        stateMachines.inputReader.TargetEvent += OnTagrget;
        stateMachines.inputReader.JumpEvent += OnJump;

        stateMachines.Animator.SetFloat(FreeLookSpeedHash, 0f);

        stateMachines.Animator.CrossFadeInFixedTime(FreeLookBlendTreeHash, CrossFadeDuration);

        if (shouldFade)
        {
            stateMachines.Animator.CrossFadeInFixedTime(FreeLookBlendTreeHash, CrossFadeDuration);
        }
        else
        {
            stateMachines.Animator.Play(FreeLookBlendTreeHash);
        }

    }
    public override void Tick(float deltaTime)
    {
        if (stateMachines.inputReader.IsAttacking)
        {
            stateMachines.SwitchState(new PlayerAttackingState(stateMachines, 0));
            return;
        }

        Vector3 movement = CalculateMovement();

        Move(movement * stateMachines.FreeLookMovementSpeed, deltaTime);

        if (stateMachines.inputReader.MovementValue == Vector2.zero)
        {
            stateMachines.Animator.SetFloat(FreeLookSpeedHash, 0, AnimatorDampTime, deltaTime);
            return;
        }

        stateMachines.Animator.SetFloat(FreeLookSpeedHash, 1, AnimatorDampTime, deltaTime);

        FaceMovementDirection(movement, deltaTime);
    }



    public override void Exit()
    {
        stateMachines.inputReader.TargetEvent -= OnTagrget;
        stateMachines.inputReader.JumpEvent -= OnJump;

    }

    private void OnTagrget()
    {
        if (!stateMachines.Targeter.SelectTarget())
        {
            return;
        }
        stateMachines.SwitchState(new PlayerTargetingState(stateMachines));
    }
    private void OnJump()
    {
        stateMachines.SwitchState(new PlayerJumpingState(stateMachines));
    }
    private Vector3 CalculateMovement()
    {
        Vector3 forward = stateMachines.MainCameraTransform.forward;
        Vector3 right = stateMachines.MainCameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        return forward * stateMachines.inputReader.MovementValue.y +
            right * stateMachines.inputReader.MovementValue.x;
    }
    private void FaceMovementDirection(Vector3 movement, float deltaTime)
    {
        stateMachines.transform.rotation = Quaternion.Lerp(
             stateMachines.transform.rotation,
             Quaternion.LookRotation(movement),
             deltaTime * stateMachines.RotationDamping);
    }
}
