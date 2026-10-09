using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Dùng 'sealed' để ngăn class khác kế thừa nó, đảm bảo tính độc nhất
public sealed class GWorld
{
    // Áp dụng mẫu thiết kế Singleton: Chỉ có duy nhất 1 GWorld tồn tại trong game[cite: 6]
    private static readonly GWorld instance = new GWorld();

    // Từ điển lưu trữ trạng thái chung của toàn bộ khu rừng
    private static WorldStates world;

    // Hàng đợi tài nguyên (Ví dụ sau này: Thức ăn, Chỗ ngủ...)
    // private static Queue<GameObject> honeyPots;

    static GWorld()
    {
        world = new WorldStates();

        /* Chỗ này sau này bạn sẽ dùng để tìm tài nguyên lúc game bắt đầu
        honeyPots = new Queue<GameObject>();
        GameObject[] honeys = GameObject.FindGameObjectsWithTag("Honey");
        foreach(GameObject h in honeys) {
            honeyPots.Enqueue(h);
        }
        if(honeys.Length > 0) {
            world.ModifyState("FreeHoney", honeys.Length);
        }
        */
    }

    private GWorld() { } // Hàm khởi tạo private để không ai có thể tạo thêm GWorld thứ 2

    // Biến toàn cục để các Agent gọi: GWorld.Instance...
    public static GWorld Instance
    {
        get { return instance; }
    }

    // Hàm lấy ra danh sách trạng thái
    public WorldStates GetWorld()
    {
        return world;
    }

    /* Sau này bạn sẽ viết thêm các hàm tương tác tài nguyên ở đây:
    public void AddHoney(GameObject h) { ... }
    public GameObject RemoveHoney() { ... }
    */
}