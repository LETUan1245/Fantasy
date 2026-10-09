using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearChasingState : BearBaseState
{
    private readonly int BlendHash = Animator.StringToHash("Blend");
    private GameObject player;

    public BearChasingState(BearStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        // BẮT BUỘC: Ép Animator thoát khỏi dáng Gầm/Đánh để trở về Blend Tree di chuyển
        stateMachine.Animator.CrossFadeInFixedTime(Animator.StringToHash("LocomotionHash"), 0.1f);
    }
    public override void Tick(float deltaTime)
    {
        if (player == null) return;

        // Tính khoảng cách giữa tâm Gấu và tâm Player
        float distance = Vector3.Distance(stateMachine.transform.position, player.transform.position);

        // 1. MẤT DẤU (Phải nới rộng ra 15m để gấu không bỏ cuộc sớm)
        if (distance > 15f)
        {
            if (stateMachine.Agent.isActiveAndEnabled && stateMachine.Agent.isOnNavMesh)
            {
                stateMachine.Agent.ResetPath();
            }

            AudioSource audio = stateMachine.GetComponent<AudioSource>();
            stateMachine.SwitchState(new BearRoarState(stateMachine, false, audio));
            return;
        }

        // 2. ĐỦ GẦN ĐỂ ĐÁNH (Chỉnh lên 3.5m hoặc 4m vì gấu to)
        if (distance <= 3.5f)
        {
            stateMachine.SwitchState(new BearAttackState(stateMachine));
            return;
        }

        // 3. RƯỢT ĐUỔI (Giữ nguyên đoạn code di chuyển mượt mà của bạn)
        if (stateMachine.Agent.isActiveAndEnabled && stateMachine.Agent.isOnNavMesh)
        {
            stateMachine.Agent.SetDestination(player.transform.position);
            Vector3 moveDirection = stateMachine.Agent.desiredVelocity.normalized;

            if (moveDirection.sqrMagnitude > 0.01f)
            {
                Move(moveDirection * stateMachine.Speed, deltaTime);
                Quaternion targetRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0, moveDirection.z));
                stateMachine.transform.rotation = Quaternion.Slerp(stateMachine.transform.rotation, targetRotation, deltaTime * 10f);

                // Phát hoạt ảnh chạy
                stateMachine.Animator.SetFloat(BlendHash, 1f, 0.1f, deltaTime);
            }
        }

        if (stateMachine.Agent.enabled)
            stateMachine.Agent.nextPosition = stateMachine.transform.position;
    }

    public override void Exit()
    {
        // Xóa đường rượt đuổi khi thoát
        if (stateMachine.Agent.enabled) stateMachine.Agent.ResetPath();
    }
}