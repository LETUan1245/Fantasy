using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearRoarState : BearBaseState
{
    // Đổi thành "Buff" cho khớp chuẩn 100% với Animator của bạn
    private readonly int RoarHash = Animator.StringToHash("Buff");
    private readonly int BlendHash = Animator.StringToHash("Blend");
    private const float CrossFadeDuration = 0.1f;

    private AudioSource audioSource;
    private bool isBuffing;

    // Dùng timer sẽ không bao giờ sợ bị lỗi kẹt hoạt ảnh
    private float timer;
    private float roarDuration = 2.0f; // Chỉnh con số này cho khớp với độ dài âm thanh gầm của bạn

    public BearRoarState(BearStateMachine stateMachine, bool isBuffing, AudioSource audio) : base(stateMachine)
    {
        this.isBuffing = isBuffing;
        this.audioSource = audio;
    }

    public override void Enter()
    {
        // Khóa chân gấu
        if (stateMachine.Agent != null)
        {
            stateMachine.Agent.isStopped = true;
            stateMachine.Agent.velocity = Vector3.zero;
        }

        // Phát hoạt ảnh "Buff"
        stateMachine.Animator.CrossFadeInFixedTime(RoarHash, CrossFadeDuration);

        // Bật tiếng
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
        }

        // Bắt đầu bấm giờ đếm ngược
        timer = roarDuration;
        // Khóa chân gấu
        if (stateMachine.Agent != null)
        {
            stateMachine.Agent.isStopped = true;
            stateMachine.Agent.velocity = Vector3.zero;
        }

        // TẮT BIẾN CHẠY ẢO: Ép biến tốc độ Blend về 0 để Animator dọn dẹp bộ nhớ chạy cũ
        stateMachine.Animator.SetFloat(BlendHash, 0f);

        // Phát hoạt ảnh Gầm (Phải đảm bảo RoarHash đã trỏ đúng tên khối trong Animator)
        stateMachine.Animator.CrossFadeInFixedTime(RoarHash, 0.1f);
    }

    public override void Tick(float deltaTime)
    {
        stateMachine.Agent.velocity = Vector3.zero;
        timer -= deltaTime;

        // Khi gầm xong 100%
        if (timer <= 0f)
        {
            if (isBuffing)
            {
                // Nếu là gầm lúc mới gặp -> Gầm xong thì ĐUỔI THEO
                stateMachine.SwitchState(new BearChasingState(stateMachine));
            }
            else
            {
                // Nếu là gầm lúc mất dấu -> Gầm xong thì VỀ IDLE (Để GOAP đi tuần tiếp)
                stateMachine.SwitchState(new BearIdleState(stateMachine));
            }
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