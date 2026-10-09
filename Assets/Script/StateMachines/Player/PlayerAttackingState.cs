using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    private float previousFrameTime;
    private Attack attack;
    private bool alreadyAppliedForce;
    public PlayerAttackingState(PlayerStateMachine stateMachines, int attackIndex) : base(stateMachines)
    {
        attack = stateMachines.Attacks[attackIndex];
    }

    public override void Enter()
    {
        stateMachines.weapon.SetAttack(attack.Damage, attack.Knockback);
        stateMachines.Animator.CrossFadeInFixedTime(attack.AnimationName, attack.TransitionDuration);
    }
    public override void Tick(float deltaTime)
    {
        FaceTarget();
        Move(deltaTime);
        float normalizedTime = GetNormalizedTime(stateMachines.Animator, "Attack");


        if (normalizedTime >= previousFrameTime && normalizedTime < 1f)
        {
            if (normalizedTime >= attack.ForceTime)
            {
                TryApplyForce();
            }
            if (stateMachines.inputReader.IsAttacking)
            {
                TryComboAttack(normalizedTime);
            }
        }
        else
        {
            if (stateMachines.Targeter.CurrentTarget != null)
            {
                stateMachines.SwitchState(new PlayerTargetingState(stateMachines));
            }
            else
            {
                stateMachines.SwitchState(new PlayerFreeLookState(stateMachines));
            }
        }

        previousFrameTime = normalizedTime;
    }

    public override void Exit()
    {

    }

    private void TryComboAttack(float normalizedTime)
    {
        if (attack.ComboStateIndex == -1) { return; }

        if (normalizedTime < attack.ComboAttackTime) { return; }

        stateMachines.SwitchState
        (
            new PlayerAttackingState
            (
                stateMachines,
                attack.ComboStateIndex
            )
        );
    }
    private void TryApplyForce()
    {
        if (alreadyAppliedForce)
        {
            return;
        }
        stateMachines.ForceReceiver.AddForce(stateMachines.transform.forward * attack.Force);
        alreadyAppliedForce = true;
    }

}
