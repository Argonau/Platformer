using UnityEngine;
using System.Collections;

public class TrollFlagTrap : MonoBehaviour
{
    [Header("1. ตั้งค่าการบิน")]
    public Rigidbody2D flagRb;
    public float flySpeedX = 8f;
    public float flySpeedY = 15f;
    public float flyDuration = 2.0f;

    [Header("2. ตั้งค่าซูมหน้าเหวอ")]
    public float zoomCloseSize = 3f;
    public float confuseDuration = 2.0f;

    private bool isTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isTriggered)
        {
            StartCoroutine(TrollSequence(other.gameObject));
        }
    }

    IEnumerator TrollSequence(GameObject player)
    {
        isTriggered = true;

        SimpleCameraFollow cam = FindFirstObjectByType<SimpleCameraFollow>();
        PlayerController controller = player.GetComponent<PlayerController>();
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();

        // --- STEP 1: ปิดการควบคุม (ปล่อยให้ลอยตามฟิสิกส์) ---
        if (controller) controller.enabled = false;

        // (เราไม่สั่งหยุดตรงนี้ เพื่อให้ตัวละครลอยโค้งสวยๆ ตามแรงโดด)

        // สร้างจุดหลอกให้กล้องมอง (เพื่อให้กล้องนิ่งสนิท)
        GameObject tempFocus = new GameObject("TempCameraFocus");
        if (cam != null)
        {
            tempFocus.transform.position = cam.transform.position;
            cam.StartCinematic(tempFocus.transform, cam.defaultSize);
        }

        // --- STEP 2: สั่งธงบินเฉียง!! ---
        if (flagRb != null)
        {
            flagRb.bodyType = RigidbodyType2D.Dynamic;
            flagRb.gravityScale = 0f;
            flagRb.linearVelocity = new Vector2(flySpeedX, flySpeedY);
        }

        // รอให้คนเล่นมองธงบิน
        yield return new WaitForSeconds(flyDuration);

        // --- STEP 3: หันขวับมาซูมหน้าคนเล่น ---
        if (cam != null)
        {
            cam.StartCinematic(player.transform, zoomCloseSize);
        }

        // *** แก้สไลด์ตรงนี้ครับ! *** // พอถึงคิวต้องเล่นบทงง สั่งหยุดการเคลื่อนที่ทุกอย่างทันที
        if (playerRb != null)
        {
            playerRb.linearVelocity = Vector2.zero; // หยุดเดิน/ไถล
            playerRb.angularVelocity = 0f;          // หยุดหมุน
        }

        // เล่นท่างง
        Animator playerAnim = player.GetComponent<Animator>();
        if (playerAnim) playerAnim.SetTrigger("Confused");

        // ยืนงง
        yield return new WaitForSeconds(confuseDuration);

        // --- STEP 4: จบคัตซีน ---
        if (cam != null) cam.StopCinematic();
        if (controller) controller.enabled = true;

        if (tempFocus) Destroy(tempFocus);
    }
}