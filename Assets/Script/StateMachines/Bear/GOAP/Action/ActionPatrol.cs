using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ActionPatrol : GAction
{
    [Header("Danh sách điểm đi tuần")]
    public GameObject[] waypoints;
    private int currentIndex = 0;

    public override bool PrePerform()
    {
        // Tự động tìm các điểm Checkpoint
        if (waypoints == null || waypoints.Length == 0)
        {
            waypoints = GameObject.FindGameObjectsWithTag("Checkpoint");
            if (waypoints.Length == 0)
            {
                Debug.LogWarning("Không tìm thấy điểm đi tuần!");
                return false;
            }
        }

        target = waypoints[currentIndex];
        return true;
    }

    public override bool PostPerform()
    {
        // Chuyển sang điểm tiếp theo khi đến nơi
        currentIndex = (currentIndex + 1) % waypoints.Length;
        return true;
    }

    // THÊM HÀM UPDATE NÀY ĐỂ CHỐNG LỖI ĐỨNG IM LÚC MỚI VÀO GAME
    private void Update()
    {
        if (running && target != null)
        {
            UnityEngine.AI.NavMeshAgent navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();

            // Đảm bảo gấu đã đứng vững trên NavMesh
            if (navAgent != null && navAgent.isActiveAndEnabled && navAgent.isOnNavMesh)
            {
                // Nếu mất đường đi (do lỗi Frame 1 hoặc do FSM vừa trả quyền), tự động vẽ lại đường!
                if (!navAgent.hasPath && !navAgent.pathPending)
                {
                    navAgent.SetDestination(target.transform.position);
                }
            }
        }
    }
}