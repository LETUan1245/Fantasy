using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ActionEat : GAction
{
    private BearStateMachine fsm;

    private void Start()
    {
        fsm = GetComponent<BearStateMachine>();
    }

    public override bool PrePerform()
    {
        target = this.gameObject; // Đứng tại chỗ để ăn

        // Ép FSM chuyển sang trạng thái ăn
        if (fsm != null) fsm.SwitchState(new BearEatState(fsm));
        return true;
    }

    public override bool PostPerform()
    {
        // 1. NẠP DẠ DÀY VÀ XÓA CẢM GIÁC ĐÓI
        BearAgent agent = GetComponent<BearAgent>();
        if (agent != null)
        {
            agent.hunger = 50f;
            agent.beliefs.RemoveState("isHungry");
        }

        // 2. DÒNG BỊ THIẾU: Trả FSM về trạng thái nghỉ để mở khóa NavMeshAgent
        if (fsm != null) fsm.SwitchState(new BearIdleState(fsm));
        return true;
    }
}