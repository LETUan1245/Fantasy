using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPullUpState : PlayerBaseState
{
    private readonly int PullUpHash = Animator.StringToHash("PullUp");
    private readonly Vector3 Offset = new Vector3(0f, 2.325f, 0.65f);
    private const float CrossFadeDuration = 0.1f;
    public PlayerPullUpState(PlayerStateMachine stateMachines) : base(stateMachines)
    {
    }

    public override void Enter()
    {
        stateMachines.Animator.CrossFadeInFixedTime(PullUpHash, CrossFadeDuration);
    }
    public override void Tick(float deltaTime)
    {
        if (GetNormalizedTime(stateMachines.Animator, "Climbing") < 1f)
        {
            return;
        }
        stateMachines.Controller.enabled = false;
        stateMachines.transform.Translate(Offset, Space.Self);
        stateMachines.Controller.enabled = true;
        stateMachines.SwitchState(new PlayerFreeLookState(stateMachines, false));
    }
    public override void Exit()
    {
        stateMachines.Controller.Move(Vector3.zero);
        stateMachines.ForceReceiver.Rest();
    }
}
