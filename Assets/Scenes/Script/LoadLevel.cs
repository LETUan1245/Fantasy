using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Bắt buộc phải thêm thư viện này để load scene

public class LoadLevel : MonoBehaviour
{
    [Tooltip("Nhập tên Scene bạn muốn chuyển tới (ví dụ: SampleScene)")]
    public string sceneName = "SampleScene";

    // Hàm này được gọi khi có một vật thể va chạm vật lý vào vật thể chứa script này
    private void OnCollisionEnter(Collision other)
    {
        // Kiểm tra xem vật thể va chạm có tag là "Player" hay không
        if (other.gameObject.CompareTag("Player"))
        {
            LoadSpecificScene();
        }
    }

    // Nếu bạn dùng Collider ở dạng IsTrigger (đi xuyên qua được như cổng dịch chuyển)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LoadSpecificScene();
        }
    }

    // Hàm load một scene cụ thể theo tên
    public void LoadSpecificScene()
    {
        SceneManager.LoadScene(sceneName);
    }

    // Hàm load scene tiếp theo dựa trên số thứ tự (Index) giống code mẫu của bạn[cite: 1]
    public void LoadNextLevelIndex()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex; 
        int nextScene = currentScene + 1; 

        if (nextScene == SceneManager.sceneCountInBuildSettings) 
        {
            nextScene = 0;
        }

        SceneManager.LoadScene(nextScene); 
    }
}