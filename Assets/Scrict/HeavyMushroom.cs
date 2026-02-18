using UnityEngine;
using System.Collections;

public class HeavyMushroom : MonoBehaviour
{
    [Header("ตั้งค่าเห็ด")]
    public float moveSpeed = 2f;
    public float giantScale = 4.0f;
    public float cameraZoomOut = 8.0f;
    public AudioClip eatSound;

    [Header("ตั้งค่าดีบัฟ (ตอนตัวใหญ่)")]
    public float heavyMoveSpeed = 1.5f; // เดินช้าลง
    public float heavyJumpForce = 0f;   // โดดไม่ได้เลย

    private Rigidbody2D rb;
    private int direction = 1;
    private bool isCollected = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb) rb.freezeRotation = true;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Item"), LayerMask.NameToLayer("Enemy"), true);
    }

    void FixedUpdate()
    {
        if (!isCollected && rb)
            rb.linearVelocity = new Vector2(moveSpeed * direction, rb.linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (isCollected) return;

        if (col.gameObject.CompareTag("Player"))
        {
            StartCoroutine(ApplyGiantEffect(col.gameObject));
        }
        else if (col.contacts.Length > 0 && Mathf.Abs(col.contacts[0].normal.x) > 0.5f)
        {
            direction *= -1;
        }
    }

    IEnumerator ApplyGiantEffect(GameObject player)
    {
        isCollected = true;

        // ซ่อนเห็ด
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        AudioSource audio = GetComponent<AudioSource>();
        if (audio && eatSound) audio.PlayOneShot(eatSound);

        // หยุดเวลาทำท่าแปลงร่าง
        Time.timeScale = 0f;

        // เก็บค่าเดิม
        Vector3 originalPos = player.transform.position;
        Vector3 originalScale = player.transform.localScale;

        // คำนวณทิศทางหันหน้า
        float facingDir = Mathf.Sign(originalScale.x);
        Vector3 targetScale = new Vector3(giantScale * facingDir, giantScale, 1f);

        // *** แก้ไขจุดที่ 1: คำนวณระยะยกตัว (เพื่อไม่ให้จมดิน) ***
        // สูตรคือ: (ขนาดใหม่ - ขนาดเก่า) / 2
        float liftOffset = (giantScale - Mathf.Abs(originalScale.y)) / 2f;
        // ตำแหน่งใหม่ตอนตัวใหญ่ (ยกขึ้นตามระยะที่คำนวณ)
        Vector3 targetPos = originalPos + new Vector3(0, liftOffset, 0);

        // เอฟเฟกต์กะพริบสลับร่าง + สลับตำแหน่ง
        for (int i = 0; i < 3; i++)
        {
            // ร่างใหญ่ + ยกตัวขึ้น
            player.transform.localScale = targetScale;
            player.transform.position = targetPos;
            yield return new WaitForSecondsRealtime(0.1f);

            // ร่างเล็ก + กลับที่เดิม
            player.transform.localScale = originalScale;
            player.transform.position = originalPos;
            yield return new WaitForSecondsRealtime(0.1f);
        }

        // --- แปลงร่างเสร็จสมบูรณ์! ---
        // ใช้ร่างใหญ่ + ตำแหน่งที่ยกขึ้นแล้ว
        player.transform.localScale = targetScale;
        player.transform.position = targetPos;

        Time.timeScale = 1f;

        // --- ใส่ดีบัฟตัวหนัก ---
        PlayerController controller = player.GetComponent<PlayerController>();
        if (controller != null)
        {
            controller.moveSpeed = heavyMoveSpeed;
            controller.jumpForce = heavyJumpForce;
        }

        // สั่งกล้องสั่น + ซูมออก
        SimpleCameraFollow cam = FindFirstObjectByType<SimpleCameraFollow>();
        if (cam)
        {
            cam.TriggerShake(0.5f, 0.5f);
            cam.SetZoom(cameraZoomOut);
        }

        Destroy(gameObject, 1f);
    }
}