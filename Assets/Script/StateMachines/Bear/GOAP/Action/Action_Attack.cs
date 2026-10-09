using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_Attack : GAction
{
    private BearStateMachine fsm;

    private void Start()
    {
        fsm = GetComponent<BearStateMachine>();
    }

    public override bool PrePerform()
    {
        // Tìm mục tiêu là Player
        target = GameObject.FindWithTag("Player");
        if (target == null) return false;

        // Bắt đầu Action: Ép FSM (Tay chân) chuyển sang trạng thái chém
        if (fsm != null) fsm.SwitchState(new BearAttackState(fsm));
        return true;
    }

    public override bool PostPerform()
    {
        // Action kết thúc: Trả FSM về trạng thái Idle để chờ lệnh tiếp theo
        if (fsm != null) fsm.SwitchState(new BearIdleState(fsm));
        return true;
    }
}