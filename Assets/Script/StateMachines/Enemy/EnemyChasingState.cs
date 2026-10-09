using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyChasingState : EnemyBaseState
{
    private readonly int LocomotionHash = Animator.StringToHash("Locomotion"); //parameters
    private readonly int SpeedHash = Animator.StringToHash("Speed"); //parameters
    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;
    public EnemyChasingState(EnemyStateMachine stateMachines) : base(stateMachines) { }

    public override void Enter()
    {
        stateMachines.Animator.CrossFadeInFixedTime(LocomotionHash, CrossFadeDuration);

    }

    public override void Tick(float deltaTime)
    {
        if (!IsInChaseRange())
        {
            stateMachines.SwitchState(new EnemyIdleState(stateMachines));
            return;
        }
        else if (IsInAttackRange())
        {
            stateMachines.SwitchState(new EnemyAttackingState(stateMachines));
        }
        MoveToPlayer(deltaTime);
        FacePlayer();
        stateMachines.Animator.SetFloat(SpeedHash, 1f, AnimatorDampTime, deltaTime);
    }
    public override void Exit()
    {
        stateMachines.Agent.ResetPath();
        stateMachines.Agent.velocity = Vector3.zero;
    }

    private void MoveToPlayer(float deltaTime)
    {
        if (stateMachines.Agent.isOnNavMesh)
        {
            stateMachines.Agent.destination = stateMachines.Player.transform.position;

            Move(stateMachines.Agent.desiredVelocity.normalized * stateMachines.MovementSpeed, deltaTime);
        }


        stateMachines.Agent.velocity = stateMachines.Controller.velocity;
    }
    private bool IsInAttackRange()
    {
        if (stateMachines.Player.IsDead) { return false; }
        float playerDistanceSqr = (stateMachines.Player.transform.position - stateMachines.transform.position).sqrMagnitude;
        return playerDistanceSqr <= stateMachines.AttackRange * stateMachines.AttackRange;
    }
}
