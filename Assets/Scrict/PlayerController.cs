using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("ตั้งค่าความเร็วและการกระโดด")]
    public float moveSpeed = 8f;
    public float jumpForce = 12f;
    public int maxJumps = 2; // กระโดดได้กี่ครั้ง (2 = Double Jump)

    [Header("ตรวจสอบพื้น")]
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask groundLayer;

    // ตัวแปรภายใน
    private Rigidbody2D rb;
    private Animator anim;
    private bool isGrounded;
    private int jumpCount;
    private float moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); // ดึง Animator มาใช้
        rb.freezeRotation = true; // ล็อกไม่ให้ตัวหมุนกลิ้ง
    }

    void Update()
    {
        // 1. รับค่าปุ่มกด (ซ้าย-ขวา)
        moveInput = Input.GetAxisRaw("Horizontal");

        // 2. เช็คพื้น
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        // 3. รีเซ็ตการกระโดดเมื่อถึงพื้น
        if (isGrounded && rb.linearVelocity.y <= 0)
        {
            jumpCount = 0;
            if (anim != null) anim.SetBool("IsJumping", false); // ปิดท่ากระโดด
        }
        else
        {
            if (anim != null) anim.SetBool("IsJumping", true); // เปิดท่ากระโดดเมื่อลอย
        }

        // 4. กดกระโดด
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // กระโดดได้ถ้า "อยู่บนพื้น" หรือ "จำนวนโดดยังไม่ครบโควต้า"
            if (isGrounded || jumpCount < maxJumps)
            {
                Jump();
            }
        }

        // 5. ส่งค่าความเร็วไปให้ Animator (เปลี่ยนท่าเดิน/ยืน)
        if (anim != null)
        {
            anim.SetFloat("Speed", Mathf.Abs(moveInput));
        }

        // 6. ระบบหันหน้าแบบใหม่ (บังคับหัน ไม่ใช้การสลับไปมา ป้องกัน Moonwalk)
        if (moveInput > 0) // กดขวา
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x); // บังคับเป็นบวกเสมอ
            transform.localScale = scale;
        }
        else if (moveInput < 0) // กดซ้าย
        {
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x); // บังคับเป็นลบเสมอ
            transform.localScale = scale;
        }
    }

    void FixedUpdate()
    {
        // สั่งเคลื่อนที่ฟิสิกส์
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    void Jump()
    {
        // รีเซ็ตความเร็วแกน Y เพื่อให้กระโดดได้ความสูงคงที่ทุกครั้ง (แม้กระโดดกลางอากาศ)
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        rb.linearVelocity += Vector2.up * jumpForce;

        jumpCount++;
    }

    // วาดวงกลมเช็คพื้นให้เห็นใน Editor
    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }
}