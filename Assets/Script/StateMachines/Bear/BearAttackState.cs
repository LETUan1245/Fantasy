using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearAttackState : BearBaseState
{
    // Đảm bảo tên "Attack" khớp với tên khối hoạt ảnh đánh trong Animator
    private readonly int AttackHash = Animator.StringToHash("Attack");

    private float timer;
    private float attackDuration = 1.5f; // Thời gian gấu vung tay xong (Chỉnh lại cho khớp hoạt ảnh của bạn)

    public BearAttackState(BearStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        // 1. Dừng chân tuyệt đối khi đánh
        if (stateMachine.Agent != null)
        {
            stateMachine.Agent.isStopped = true;
            stateMachine.Agent.velocity = Vector3.zero;
        }

        // 2. Phát hoạt ảnh đánh
        stateMachine.Animator.CrossFadeInFixedTime(AttackHash, 0.1f);

        // 3. Khởi động đồng hồ đếm ngược
        timer = attackDuration;
    }

    public override void Tick(float deltaTime)
    {
        // Trừ thời gian
        timer -= deltaTime;

        // Khi đồng hồ về 0 -> Chắc chắn 100% thoát khỏi trạng thái đánh
        if (timer <= 0f)
        {
            // Đánh xong thì quay lại rượt đuổi để kiểm tra xem Player còn ở gần không
            stateMachine.SwitchState(new BearChasingState(stateMachine));
        }
    }

    public override void Exit()
    {
        // Mở khóa đôi chân
        if (stateMachine.Agent != null)
        {
            stateMachine.Agent.isStopped = false;
        }
    }
}