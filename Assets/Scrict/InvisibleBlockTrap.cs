using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class InvisibleBlockTrap : MonoBehaviour
{
    private Tilemap tilemap;

    [Header("ตั้งค่าบล็อก")]
    public TileBase invisibleTile; // บล็อกเปล่าๆ (ไม่มีรูป)
    public TileBase visibleTile;   // บล็อกที่จะให้โชว์ตอนโดนโหม่ง
    public float hideDelay = 2.0f;

    void Start()
    {
        tilemap = GetComponent<Tilemap>();

        // --- ส่วนที่เพิ่ม: บังคับให้บล็อกทุกลูกใน Layer นี้ล่องหนทันทีที่เริ่มเกม ---
        // มันจะไล่เปลี่ยนทุกจุดที่คุณวาดไว้ให้กลายเป็นรูป "ล่องหน" ครับ
        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            if (tilemap.HasTile(pos))
            {
                tilemap.SetTile(pos, invisibleTile);
            }
        }

        // มั่นใจว่าสีต้องชัดเจน เพราะเราสลับรูปเอา
        Color c = tilemap.color;
        c.a = 1;
        tilemap.color = c;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            foreach (ContactPoint2D hit in collision.contacts)
            {
                // เช็กทิศทางการโหม่ง
                if (hit.point.y > collision.transform.position.y)
                {
                    Vector3 checkPos = hit.point + (Vector2.up * 0.2f);
                    Vector3Int cellPosition = tilemap.WorldToCell(checkPos);

                    // ถ้าโหม่งโดนช่องที่เป็น "บล่องหน" ให้แสดงตัว
                    if (tilemap.GetTile(cellPosition) == invisibleTile)
                    {
                        StartCoroutine(ShowAndHideBlock(cellPosition));
                    }
                }
            }
        }
    }

    IEnumerator ShowAndHideBlock(Vector3Int pos)
    {
        tilemap.SetTile(pos, visibleTile); // โชว์บล็อก
        yield return new WaitForSeconds(hideDelay); // รอเวลา
        tilemap.SetTile(pos, invisibleTile); // กลับไปล่องหน
    }
}