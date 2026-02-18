using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverUI;

    void Start()
    {
        if (gameOverUI != null) gameOverUI.SetActive(false);
    }

    public void PlayerDied()
    {
        // ฟังก์ชันนี้จะถูกเรียกหลังจาก Player เล่นท่าตายจบแล้ว
        if (gameOverUI != null) gameOverUI.SetActive(true);

        // หยุดเวลาตอนนี้แหละ เพื่อให้เมนู Game Over ใช้งานได้ปกติ
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}