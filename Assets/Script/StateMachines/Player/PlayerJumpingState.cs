using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerJumpingState : PlayerBaseState
{
    private readonly int JumpHash = Animator.StringToHash("Jump");
    private Vector3 momentum;
    private const float CrossFadeDuration = 0.1f;
    public PlayerJumpingState(PlayerStateMachine stateMachines) : base(stateMachines)
    {
    }

    public override void Enter()
    {
        stateMachines.ForceReceiver.Jump(stateMachines.JumpForce);
        momentum = stateMachines.Controller.velocity;
        momentum.y = 0f;
        stateMachines.Animator.CrossFadeInFixedTime(JumpHash, CrossFadeDuration);

        stateMachines.LedgeDetector.OnLedgeDetect += HandleLedgeDetect;
    }
    public override void Tick(float deltaTime)
    {
        Move(momentum, deltaTime);
        if (stateMachines.Controller.velocity.y <= 0)
        {
            stateMachines.SwitchState(new PlayerFallingState(stateMachines));
            return;
        }
        FaceTarget();
    }
    public override void Exit()
    {
        stateMachines.LedgeDetector.OnLedgeDetect -= HandleLedgeDetect;
    }

    private void HandleLedgeDetect(Vector3 ledgeForward, Vector3 closetPoint)
    {
        stateMachines.SwitchState(new PlayerHangingState(stateMachines, ledgeForward, closetPoint));
    }


}
