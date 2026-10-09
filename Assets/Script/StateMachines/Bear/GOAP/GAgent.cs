using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
// Class định nghĩa một Mục tiêu (Goal)
public class SubGoal
{
    public Dictionary<string, int> sGoals;
    public bool remove;

    public SubGoal(string s, int i, bool r)
    {
        sGoals = new Dictionary<string, int>();
        sGoals.Add(s, i);
        remove = r; // Có xóa mục tiêu này đi sau khi hoàn thành không?
    }
}

public class GAgent : MonoBehaviour
{
    public List<GAction> actions = new List<GAction>();
    public Dictionary<SubGoal, int> goals = new Dictionary<SubGoal, int>();
    public WorldStates beliefs = new WorldStates();

    // Tạm khóa GInventory chờ thiết lập sau
    // public GInventory inventory = new GInventory(); 

    bool invoked = false;

    GPlanner planner;
    Queue<GAction> actionQueue;
    public GAction currentAction;
    SubGoal currentGoal;

    protected virtual void Start()
    {
        // 1. Tự động dò tìm tất cả các Hành động (GAction) đang gắn trên chú gấu
        GAction[] acts = this.GetComponents<GAction>();
        foreach (GAction a in acts)
        {
            actions.Add(a);
        }
    }

    void CompleteAction()
    {
        currentAction.running = false;
        currentAction.PostPerform();
        invoked = false;
    }

    void LateUpdate()
    {
        // 2. KHI ĐANG THỰC THI HÀNH ĐỘNG
        if (currentAction != null && currentAction.running)
        {
            // Đã thêm kiểm tra !pathPending để tránh lỗi "hồn lìa khỏi xác" của NavMesh[cite: 1]
            if (!currentAction.agent.pathPending && currentAction.agent.remainingDistance < 1.0f)
            {
                if (!invoked)
                {
                    // Chờ hết thời gian làm việc (duration) rồi đánh dấu hoàn thành[cite: 1]
                    Invoke("CompleteAction", currentAction.duration);
                    invoked = true;
                }
            }
            return;
        }

        // 3. KHI ĐANG RẢNH RỖI -> XIN KẾ HOẠCH MỚI
        if (planner == null || actionQueue == null)
        {
            planner = new GPlanner();

            // Sắp xếp Goal theo độ ưu tiên (Priority) từ cao xuống thấp bằng LINQ[cite: 1]
            var sortedGoals = from entry in goals orderby entry.Value descending select entry;

            foreach (KeyValuePair<SubGoal, int> sg in sortedGoals)
            {
                // Gọi Planner. Truyền tạm new WorldStates() chờ tạo GWorld toàn cục sau
                actionQueue = planner.plan(actions, sg.Key.sGoals, new WorldStates(), beliefs);

                if (actionQueue != null)
                {
                    currentGoal = sg.Key;
                    break; // Đã tìm thấy kế hoạch thì thoát vòng lặp
                }
            }
        }

        // 4. XÓA MỤC TIÊU NẾU ĐÃ ĐẠT ĐƯỢC
        if (actionQueue != null && actionQueue.Count == 0)
        {
            if (currentGoal.remove)
            {
                goals.Remove(currentGoal);
            }
            planner = null;
        }

        // 5. BẮT ĐẦU LẤY HÀNH ĐỘNG RA LÀM
        if (actionQueue != null && actionQueue.Count > 0)
        {
            currentAction = actionQueue.Dequeue();

            if (currentAction.PrePerform())
            {
                // Nếu chưa có tọa độ đích, tự động tìm bằng Tag[cite: 1]
                if (currentAction.target == null && currentAction.targetTag != "")
                {
                    currentAction.target = GameObject.FindWithTag(currentAction.targetTag);
                }

                if (currentAction.target != null)
                {
                    currentAction.running = true;
                    // Ép NavMesh di chuyển đến mục tiêu
                    currentAction.agent.SetDestination(currentAction.target.transform.position);
                }
            }
            else
            {
                // Nếu Action không thỏa mãn điều kiện thực thi -> Hủy Kế Hoạch
                actionQueue = null;
            }
        }
    }
}