using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    public GameObject enemy; // ลากศัตรูที่อยู่ข้างบนมาใส่ช่องนี้

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && enemy != null)
        {
            // เปลี่ยนให้ศัตรูใช้ฟิสิกส์ปกติเพื่อให้ร่วงลงมา
            Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
            if (rb)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.gravityScale = 3f; // ให้ร่วงเร็วๆ แบบสะใจ
            }
            Destroy(gameObject); // ใช้ครั้งเดียวทิ้ง
        }
    }
}