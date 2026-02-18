using UnityEngine;

public class WeakFloorTrap : MonoBehaviour
{
    [Header("ตั้งค่าพื้นพัง")]
    public float breakDelay = 0.5f;
    public AudioClip crumbleSound;

    [Header("เพื่อนตาย (ลากบล็อกที่อยู่ข้างใต้มาใส่ตรงนี้)")]
    public GameObject[] extraObjectsToBreak; // *** พระเอกของเรา ***

    private bool isBreaking = false;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isBreaking)
        {
            // เช็กว่าตัวใหญ่หรือยัง (Scale > 2.5)
            if (collision.transform.localScale.x >= 2.5f)
            {
                StartCoroutine(BreakFloor());
            }
        }
    }

    System.Collections.IEnumerator BreakFloor()
    {
        isBreaking = true;

        AudioSource audio = GetComponent<AudioSource>();
        if (audio && crumbleSound) audio.PlayOneShot(crumbleSound);

        // สั่นพื้นหลักเตือนก่อน
        Vector3 originalPos = transform.position;
        float timer = 0;

        // (แถม) สั่นเพื่อนๆ ด้วย
        Vector3[] extrasOriginalPos = new Vector3[extraObjectsToBreak.Length];
        for (int i = 0; i < extraObjectsToBreak.Length; i++)
            if (extraObjectsToBreak[i]) extrasOriginalPos[i] = extraObjectsToBreak[i].transform.position;

        while (timer < breakDelay)
        {
            // สั่นพื้นหลัก
            transform.position = originalPos + (Vector3)(Random.insideUnitCircle * 0.05f);

            // สั่นเพื่อนๆ
            for (int i = 0; i < extraObjectsToBreak.Length; i++)
            {
                if (extraObjectsToBreak[i])
                    extraObjectsToBreak[i].transform.position = extrasOriginalPos[i] + (Vector3)(Random.insideUnitCircle * 0.05f);
            }

            timer += Time.deltaTime;
            yield return null;
        }

        // --- ถึงเวลาพังยับเยิน! ---
        // 1. พังตัวเอง
        DropObject(gameObject);

        // 2. พังเพื่อนๆ ที่ผูกไว้
        foreach (GameObject obj in extraObjectsToBreak)
        {
            if (obj != null) DropObject(obj);
        }
    }

    // ฟังก์ชันช่วยทำให้ของร่วง
    void DropObject(GameObject obj)
    {
        // ใส่ Rigidbody ให้ร่วง
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        if (!rb) rb = obj.AddComponent<Rigidbody2D>();

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 4f;       // ร่วงไวๆ
        rb.angularVelocity = Random.Range(-100f, 100f); // หมุนมั่วๆ

        // ปิด Collider (เพื่อให้ผู้เล่นร่วงทะลุผ่านลงไปได้)
        Collider2D col = obj.GetComponent<Collider2D>();
        if (col) col.enabled = false;

        // ทำลายทิ้งเมื่อตกไปไกล
        Destroy(obj, 3f);
    }
}