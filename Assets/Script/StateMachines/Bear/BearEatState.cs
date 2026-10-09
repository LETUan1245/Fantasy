using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearEatState : BearBaseState
{
    // Bắt chính xác tên khối trạng thái trong hình image_e48fac.png
    private readonly int EatHash = Animator.StringToHash("Eat");
    private const float CrossFadeDuration = 0.1f;

    public BearEatState(BearStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        // 1. Phát hoạt ảnh ăn ngay khi bước vào State
        stateMachine.Animator.CrossFadeInFixedTime(EatHash, CrossFadeDuration);

        // 2. Ép NavMeshAgent dừng lại để gấu không bị trôi trượt khi đang ăn
        stateMachine.Agent.isStopped = true;
        stateMachine.Agent.velocity = Vector3.zero;
    }

    public override void Tick(float deltaTime)
    {
        // Không cần viết logic đếm thời gian ở đây vì GOAP Action_Eat sẽ tự đếm
    }

    public override void Exit()
    {
        // Cho phép NavMesh hoạt động lại khi kết thúc việc ăn
        stateMachine.Agent.isStopped = false;
    }
}
