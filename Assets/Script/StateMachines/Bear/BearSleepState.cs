using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearSleepState : BearBaseState
{
    private readonly int SleepHash = Animator.StringToHash("Sleep");
    private const float CrossFadeDuration = 0.2f;

    public BearSleepState(BearStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        // 1. Phát hoạt ảnh ngủ (thời gian chuyển mượt dài hơn một chút để gấu từ từ nằm xuống)
        stateMachine.Animator.CrossFadeInFixedTime(SleepHash, CrossFadeDuration);

        // 2. Khóa di chuyển
        stateMachine.Agent.isStopped = true;
        stateMachine.Agent.velocity = Vector3.zero;
    }

    public override void Tick(float deltaTime)
    {
        // Ngủ say sưa, chờ lệnh đánh thức từ GOAP
    }

    public override void Exit()
    {
        // Mở khóa di chuyển khi thức dậy
        stateMachine.Agent.isStopped = false;
    }
}
