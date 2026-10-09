using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class GAction : MonoBehaviour
{
    public string actionName = "Action";
    public float cost = 1.0f;
    public GameObject target;
    public string targetTag;
    public float duration = 0.0f;

    // Mảng để lập trình viên kéo thả dễ dàng trên cửa sổ Inspector của Unity
    public WorldState[] preConditions;
    public WorldState[] afterEffects;

    public NavMeshAgent agent;

    // Dictionary để Planner tra cứu tốc độ cao ở chế độ ẩn (Runtime)
    public Dictionary<string, int> preconditions;
    public Dictionary<string, int> effects;

    public bool running = false;

    /* Tạm khóa lại chờ đến Bước 4 (Tạo GAgent) sẽ mở ra
    public GInventory inventory;
    public WorldStates beliefs;
    */

    public GAction()
    {
        preconditions = new Dictionary<string, int>();
        effects = new Dictionary<string, int>();
    }

    private void Awake()
    {
        agent = this.gameObject.GetComponent<NavMeshAgent>();

        // Chuyển đổi dữ liệu từ Mảng (Inspector) sang Dictionary (Bộ nhớ)[cite: 7]
        if (preConditions != null)
        {
            foreach (WorldState w in preConditions)
            {
                preconditions.Add(w.key, w.value);
            }
        }

        if (afterEffects != null)
        {
            foreach (WorldState w in afterEffects)
            {
                effects.Add(w.key, w.value);
            }
        }
    }

    public bool IsAchievable()
    {
        return true;
    }

    // Hàm để Planner kiểm tra xem Effect của Action trước có khớp Precondition của Action này không[cite: 7]
    public bool IsAchievableGiven(Dictionary<string, int> conditions)
    {
        foreach (KeyValuePair<string, int> p in preconditions)
        {
            if (!conditions.ContainsKey(p.Key)) return false;
        }
        return true;
    }

    // 2 Hàm trừu tượng để sau này gắn logic FSM (như phát hoạt ảnh)[cite: 7]
    public abstract bool PrePerform();
    public abstract bool PostPerform();
}
