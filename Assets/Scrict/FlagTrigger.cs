using UnityEngine;

public class FlagTrigger : MonoBehaviour
{
    public TrollFlag flagScript; // ลากตัวธงมาใส่ในช่องนี้

    private void OnTriggerEnter2D(Collider2D other)
    {
        // ถ้าผู้เล่นเดินมาถึงจุดที่กำหนด
        if (other.CompareTag("Player") && flagScript != null)
        {
            flagScript.StartFlying(); // สั่งให้ธงบินหนี!
        }
    }
}