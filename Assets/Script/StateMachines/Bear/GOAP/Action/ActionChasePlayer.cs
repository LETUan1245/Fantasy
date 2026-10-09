using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionChasePlayer : GAction
{
    private BearStateMachine fsm;
    private AudioSource audioSource;
    private BearAgent agent;

    private void Start()
    {
        fsm = GetComponent<BearStateMachine>();
        audioSource = GetComponent<AudioSource>();
        agent = GetComponent<BearAgent>();
    }

    public override bool PrePerform()
    {
        target = GameObject.FindWithTag("Player");
        if (target == null) return false;

        // NGAY KHI VỪA THẤY NGƯỜI CHƠI: Ép FSM gầm uy hiếp (isBuffing = true)
        if (fsm != null)
        {
            fsm.SwitchState(new BearRoarState(fsm, true, audioSource));
        }
        return true;
    }

    public override bool PostPerform()
    {
        // KHI HÀNH ĐỘNG KẾT THÚC (Hoặc do đuổi kịp, hoặc do mất dấu)
        if (target != null && agent != null)
        {
            // Kiểm tra xem gấu có bị mất dấu người chơi không (Dựa vào belief của BearAgent)
            if (!agent.beliefs.HasState("playerInSight"))
            {
                // MẤT DẤU NGƯỜI CHƠI: Ép FSM gầm tức giận (isBuffing = false)
                if (fsm != null)
                {
                    fsm.SwitchState(new BearRoarState(fsm, false, audioSource));
                }
            }
        }
        return true;
    }
    // Thêm hàm Update để bám đuôi mục tiêu di động
    private void Update()
    {
        if (running && target != null)
        {
            UnityEngine.AI.NavMeshAgent navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (navAgent != null && navAgent.isActiveAndEnabled && navAgent.isOnNavMesh)
            {
                // THÊM DÒNG NÀY: Tránh xung đột với BearRoarState
                // Nếu FSM đang ép dừng (lúc gầm), thì không set đường đi nữa
                if (navAgent.isStopped) return;

                // Chỉ vẽ đường mới nếu Player di chuyển ra xa hơn 1 mét
                if (Vector3.Distance(navAgent.destination, target.transform.position) > 1.0f)
                {
                    navAgent.SetDestination(target.transform.position);
                }
            }
        }
    }
}