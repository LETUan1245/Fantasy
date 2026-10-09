using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerHangingState : PlayerBaseState
{
    private readonly int HangingHash = Animator.StringToHash("Hanging");
    private const float CrossFadeDuration = 0.1f;
    private Vector3 closetPoint;


    private Vector3 ledgeForward;
    public PlayerHangingState(PlayerStateMachine stateMachines, Vector3 ledgeForward, Vector3 closetPoint) : base(stateMachines)
    {

        this.ledgeForward = ledgeForward;
        this.closetPoint = closetPoint;
    }

    public override void Enter()
    {
        stateMachines.transform.rotation = Quaternion.LookRotation(ledgeForward, Vector3.up);
        stateMachines.Controller.enabled = false;
        stateMachines.transform.position = closetPoint - (stateMachines.LedgeDetector.transform.position - stateMachines.transform.position);
        stateMachines.Controller.enabled = true;

        stateMachines.Animator.CrossFadeInFixedTime(HangingHash, CrossFadeDuration);
    }
    public override void Tick(float deltaTime)
    {
        if (stateMachines.inputReader.MovementValue.y > 0f)
        {
            stateMachines.SwitchState(new PlayerPullUpState(stateMachines));
        }
        else if (stateMachines.inputReader.MovementValue.y < 0f)
        {
            stateMachines.Controller.Move(Vector3.zero);
            stateMachines.ForceReceiver.Rest();
            stateMachines.SwitchState(new PlayerFallingState(stateMachines));
        }
    }



    public override void Exit()
    {

    }


}