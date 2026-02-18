using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    private float length, startpos;
    public GameObject cam;           // ลากกล้องที่มีสคริปต์ SimpleCameraFollow มาใส่
    public float parallaxFactor;     // ค่าความเร็ว (0 = อยู่ไกลสุดไม่ขยับ, 1 = ขยับตามกล้องเป๊ะ)

    void Start()
    {
        startpos = transform.position.x;
        // คำนวณความกว้างของรูปภาพ
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        // 1. คำนวณระยะที่ "ภาพควรจะอยู่" สัมพันธ์กับกล้อง
        float distance = (cam.transform.position.x * parallaxFactor);

        // 2. ขยับภาพไปตามตำแหน่งที่คำนวณ
        transform.position = new Vector3(startpos + distance, transform.position.y, transform.position.z);

        // 3. ระบบ Loop (ถ้าเดินเลยขอบภาพ ให้วาร์ปภาพไปต่อข้างหน้า)
        float temp = (cam.transform.position.x * (1 - parallaxFactor));
        if (temp > startpos + length) startpos += length;
        else if (temp < startpos - length) startpos -= length;
    }
}