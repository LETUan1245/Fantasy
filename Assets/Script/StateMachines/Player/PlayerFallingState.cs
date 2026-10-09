using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : PlayerBaseState
{
    private readonly int FallHash = Animator.StringToHash("Fall");
    private const float CrossFadeDuration = 0.1f;
    private Vector3 momentum;
    public PlayerFallingState(PlayerStateMachine stateMachines) : base(stateMachines)
    {

    }

    public override void Enter()
    {
        momentum = stateMachines.Controller.velocity;
        momentum.y = 0f;

        stateMachines.Animator.CrossFadeInFixedTime(FallHash, CrossFadeDuration);
        stateMachines.LedgeDetector.OnLedgeDetect += HandleLedgeDetect;
    }
    public override void Tick(float deltaTime)
    {
        Move(momentum, deltaTime);

        if (stateMachines.Controller.isGrounded)
        {
            ReturnToLocomotion();
        }

        FaceTarget();
    }
    public override void Exit()
    {
        stateMachines.LedgeDetector.OnLedgeDetect += HandleLedgeDetect;
    }
    private void HandleLedgeDetect(Vector3 ledgeForward, Vector3 closetPoint)
    {
        stateMachines.SwitchState(new PlayerHangingState(stateMachines, ledgeForward, closetPoint));
    }

}
