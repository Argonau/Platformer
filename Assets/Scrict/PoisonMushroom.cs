using UnityEngine;
using System.Collections;

public class PoisonMushroom : MonoBehaviour
{
    public float shrinkMultiplier = 0.4f; // ตัวหดเหลือ 40%
    public float slowMultiplier = 0.5f;   // วิ่งช้าลงครึ่งนึง

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(ApplyPoison(other.gameObject));
        }
    }

    IEnumerator ApplyPoison(GameObject player)
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        // ดึงสคริปต์มาจัดการ
        PlayerController move = player.GetComponent<PlayerController>();

        // ตัวละครหดเล็กลงแทนที่จะขยายใหญ่
        player.transform.localScale *= shrinkMultiplier;
        if (move) move.moveSpeed *= slowMultiplier;

        // ทำเอฟเฟกต์สีม่วงๆ ให้รู้ว่าโดนพิษ
        SpriteRenderer sr = player.GetComponent<SpriteRenderer>();
        if (sr) sr.color = Color.magenta;

        yield return new WaitForSeconds(3f); // โดนพิษ 3 วินาที

        // คืนค่า
        player.transform.localScale /= shrinkMultiplier;
        if (move) move.moveSpeed /= slowMultiplier;
        if (sr) sr.color = Color.white;

        Destroy(gameObject);
    }
}