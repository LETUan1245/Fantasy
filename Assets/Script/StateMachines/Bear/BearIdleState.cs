using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearIdleState : BearBaseState
{
    private readonly int BlendHash = Animator.StringToHash("Blend");
    private readonly int LocomotionHash = Animator.StringToHash("LocomotionHash");

    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;

    public BearIdleState(BearStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        // --- 1. RA ĐA CẢNH GIỚI (Giữ nguyên như bạn đã viết trước đó nếu cần) ---
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            float distance = Vector3.Distance(stateMachine.transform.position, player.transform.position);
            if (distance <= 10f)
            {
                if (stateMachine.Agent.enabled) stateMachine.Agent.ResetPath();
                AudioSource audio = stateMachine.GetComponent<AudioSource>();
                stateMachine.SwitchState(new BearRoarState(stateMachine, true, audio));
                return;
            }
        }

        // --- 2. LOGIC ĐI TUẦN THEO GOAP (Sửa lỗ hổng vật lý ở đây) ---
        // Kiểm tra xem GOAP (ActionPatrol) có đang ra lệnh đi không
        bool isMoving = stateMachine.Agent.pathPending ||
                       (stateMachine.Agent.hasPath && stateMachine.Agent.remainingDistance > stateMachine.Agent.stoppingDistance);

        if (isMoving)
        {
            // Lấy hướng dẫn đường từ GOAP
            Vector3 moveDirection = stateMachine.Agent.desiredVelocity.normalized;

            // CHỈ di chuyển và xoay nếu vận tốc hướng dẫn đủ lớn
            if (moveDirection.sqrMagnitude > 0.01f)
            {
                Move(moveDirection * stateMachine.Speed, deltaTime);

                // Xoay mặt mượt mà theo đường đi
                Quaternion targetRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0, moveDirection.z));
                stateMachine.transform.rotation = Quaternion.Slerp(stateMachine.transform.rotation, targetRotation, deltaTime * 10f);

                // Phát hoạt ảnh chạy/đi bộ dựa trên vận tốc
                float speedPercent = stateMachine.Agent.velocity.magnitude / stateMachine.Speed;
                stateMachine.Animator.SetFloat(BlendHash, speedPercent, AnimatorDampTime, deltaTime);
            }
        }
        else
        {
            // Đã đến đích Checkpoint -> DỪNG HẲN HOẠT ẢNH
            Move(Vector3.zero, deltaTime);
            stateMachine.Animator.SetFloat(BlendHash, 0f, AnimatorDampTime, deltaTime);
        }

        // Ép NavMesh bám sát cơ thể để giữ đồng bộ vật lý
        if (stateMachine.Agent.enabled)
        {
            stateMachine.Agent.nextPosition = stateMachine.transform.position;
        }
    }

    public override void Exit() { }
}