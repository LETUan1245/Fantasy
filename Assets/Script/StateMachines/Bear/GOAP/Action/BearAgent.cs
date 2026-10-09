using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BearAgent : GAgent
{
    [Header("Chỉ số Sinh tồn")]
    public float hunger = 100f;
    private const float HUNGER_THRESHOLD = 30f; // Dưới 30 là bắt đầu đói
    public float sightRange = 10f; // Tầm nhìn thấy Player

    private GameObject player;

    protected override void Start()
    {
        base.Start();
        player = GameObject.FindWithTag("Player");

        // Goal 1: Đi tuần tra (Ưu tiên 1 - Thấp nhất, làm lúc rảnh rỗi)
        SubGoal patrolGoal = new SubGoal("isPatrolling", 1, false);
        goals.Add(patrolGoal, 1);

        // Goal 2: Đi ngủ (Ưu tiên 3 - Trung bình, chỉ làm được khi Đói -> Ăn -> Ngủ)
        SubGoal restGoal = new SubGoal("isRested", 1, false);
        goals.Add(restGoal, 3);

        // Goal 3: Giết kẻ thù (Ưu tiên 10 - Tuyệt đối, bỏ hết mọi việc để làm)
        SubGoal attackGoal = new SubGoal("playerDead", 1, false);
        goals.Add(attackGoal, 10);
    }

    private void Update()
    {
        // 1. CƠ CHẾ ĐÓI BỤNG (Giữ nguyên như của bạn)
        if (hunger > 0) hunger -= Time.deltaTime;
        if (hunger <= HUNGER_THRESHOLD)
        {
            if (!beliefs.HasState("isHungry")) beliefs.SetState("isHungry", 1);
        }

        // 2. Cơ chế cảm biến thấy và áp sát người chơi
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);

            if (distance <= sightRange)
            {
                // ... (Giữ nguyên đoạn code điều kiện <= sightRange của bạn) ...
            }
            else
            {
                // THÊM ĐOẠN NÀY: Kiểm tra xem frame trước đó có đang thấy người chơi không
                if (beliefs.HasState("playerInSight"))
                {
                    // Vừa mất dấu -> Ép FSM chuyển sang trạng thái gầm
                    var fsm = GetComponent<BearStateMachine>();
                    var audioSrc = GetComponent<AudioSource>();
                    if (fsm != null)
                    {
                        // Tham số false ở đây biểu thị là gầm tức giận do mất dấu
                        fsm.SwitchState(new BearRoarState(fsm, false, audioSrc));
                    }
                }

                // Đảm bảo xóa cờ để GOAP chuyển về Patrol
                beliefs.RemoveState("playerInSight");
                beliefs.RemoveState("nearPlayer"); // Ở xa thì xóa điều kiện đánh
            }
        }
    }
    private void OnGUI()
    {
        // Định dạng chữ to, in đậm và màu vàng cho dễ nhìn
        GUIStyle style = new GUIStyle();
        style.fontSize = 25;
        style.fontStyle = FontStyle.Bold;
        style.normal.textColor = Color.yellow;

        // In ra chỉ số đói ở góc trái màn hình
        GUI.Label(new Rect(20, 20, 300, 50), "Độ đói của gấu: " + Mathf.RoundToInt(hunger), style);

        // In ra hành động hiện tại để biết khi nào nó quyết định đi ăn
        if (currentAction != null)
        {
            GUI.Label(new Rect(20, 60, 400, 50), "Trạng thái GOAP: " + currentAction.actionName, style);
        }
    }
}
