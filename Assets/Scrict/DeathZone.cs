using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. ถ้าคนตกเป็นผู้เล่น
        if (other.CompareTag("Player"))
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null)
            {
                // บังคับลดเลือดจนตายทันทีเพื่อให้เล่นท่าตาย
                health.TakeDamage(999);
            }
        }
        // 2. ถ้าเป็นศัตรูหรือไอเทมตกหลุม ให้ลบทิ้งไปเลยเพื่อลดภาระเครื่อง
        else
        {
            Destroy(other.gameObject);
        }
    }
}