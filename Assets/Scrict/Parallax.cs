using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, startpos;
    public GameObject cam; // ลากกล้องมาใส่ตรงนี้
    public float parallaxEffect; // ค่าความเร็วที่เราตั้งในตาราง Step 1

    void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x; // วัดความกว้างภาพ
    }

    void Update()
    {
        // ระยะที่ภาพควรจะขยับไป (อิงจากกล้อง)
        float dist = (cam.transform.position.x * parallaxEffect);

        // ระยะที่ใช้เช็กว่าภาพหลุดขอบหรือยัง (สำหรับทำ Loop)
        float temp = (cam.transform.position.x * (1 - parallaxEffect));

        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        // เทคนิค "วาร์ปภาพ" ให้เดินได้ไม่สิ้นสุด
        if (temp > startpos + length) startpos += length;
        else if (temp < startpos - length) startpos -= length;
    }
}