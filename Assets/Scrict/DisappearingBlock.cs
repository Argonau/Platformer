using UnityEngine;

public class DisappearingBlock : MonoBehaviour
{
    // ทำงานเมื่อชนแบบของแข็ง (ไม่ได้ติ๊ก Is Trigger)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // --- ส่วนที่เพิ่มเข้ามา ---
            // ตรวจสอบทิศทาง: ถ้าชนแล้วพัง ต้องเป็นการชนจาก "ด้านล่าง" เท่านั้น
            // โดยเช็คว่า ผู้เล่น (collision) อยู่ต่ำกว่า บล็อก (transform) หรือไม่
            if (collision.transform.position.y < transform.position.y)
            {
                // ตรวจสอบเพิ่มเติม: ป้องกันกรณีชนขอบข้างๆ แล้วพัง
                // ดูจุดที่ชนกัน (Contact Point) ว่าแกน Y ของผู้เล่นต้องอยู่ต่ำจริงๆ
                foreach (ContactPoint2D contact in collision.contacts)
                {
                    // ถ้าจุดที่ชน (normal) ชี้ขึ้นฟ้า (y > 0.5) แสดงว่าหัวผู้เล่นกระแทกตูดบล็อก
                    if (contact.normal.y > 0.5f)
                    {
                        gameObject.SetActive(false);
                        break; // เจอจุดชนที่ถูกต้องแล้ว หยุดเช็ค
                    }
                }
            }
        }
    }
}