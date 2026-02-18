using UnityEngine;

public class TrollFlag : MonoBehaviour
{
    public float flySpeed = 15f;    // ความเร็วตอนบินหนี
    public float rotateSpeed = 500f; // หมุนติ้วๆ เพิ่มความกวน
    private bool isFlying = false;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isFlying)
        {
            // หมุนตัวธงให้ดูเหมือนมันปลิวไปตามลมแบบกวนๆ
            transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
        }
    }

    // ฟังก์ชันนี้จะถูกเรียกจาก Trigger Zone ข้างหน้าเสา
    public void StartFlying()
    {
        if (!isFlying)
        {
            isFlying = true;
            if (rb != null)
            {
                // เปลี่ยนเป็น Dynamic เพื่อให้มันเคลื่อนที่ตามฟิสิกส์ได้
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.gravityScale = -1.5f; // ทำให้มันเบาจนลอยขึ้นฟ้า
                rb.linearVelocity = new Vector2(5f, flySpeed); // บินเฉียงขึ้นไปทางขวา
            }
            Debug.Log("ธง: บายจ้าาาา!");
        }
    }
}