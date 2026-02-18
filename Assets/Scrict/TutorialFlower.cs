using UnityEngine;
using TMPro; // เรียกใช้ TextMeshPro
using System.Collections;

public class TutorialFlower : MonoBehaviour
{
    [Header("ตั้งค่ารูปภาพ")]
    public Sprite bloomSprite; // รูปตอนบาน
    private Sprite initialSprite; // รูปเดิม (เก็บไว้คืนค่าตอนเดินออก)
    private SpriteRenderer sr;

    [Header("ตั้งค่าข้อความ")]
    public GameObject dialogueBox; // ลาก TutorialBox มาใส่
    public TMP_Text textComponent; // ลาก TutorialText มาใส่

    [TextArea(3, 10)] // ทำให้ช่องพิมพ์ใน Inspector กว้างขึ้น
    public string message = "กด A เดินซ้าย\nกด D เดินขวา\nกด Spacebar เพื่อกระโดด";

    public float typingSpeed = 0.05f; // ความเร็วในการพิมพ์ทีละตัว

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        initialSprite = sr.sprite; // จำรูปเดิมไว้
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 1. เปลี่ยนรูปเป็นดอกไม้บาน
            if (bloomSprite != null) sr.sprite = bloomSprite;

            // 2. เปิดกล่องข้อความ
            dialogueBox.SetActive(true);

            // 3. เริ่มพิมพ์ข้อความ (Typewriter Effect)
            StopAllCoroutines(); // กันบั๊กถ้าเดินเข้าออกเร็วๆ
            StartCoroutine(TypeLine());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 1. คืนรูปเดิม (ถ้าอยากให้หุบ)
            // sr.sprite = initialSprite; 

            // 2. ปิดกล่องข้อความ
            dialogueBox.SetActive(false);
            textComponent.text = ""; // เคลียร์ข้อความ
            StopAllCoroutines(); // หยุดพิมพ์ทันที
        }
    }

    IEnumerator TypeLine()
    {
        textComponent.text = ""; // ล้างข้อความเก่าก่อน

        // ลูปพิมพ์ทีละตัวอักษร
        foreach (char letter in message.ToCharArray())
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}