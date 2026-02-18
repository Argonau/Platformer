using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapBreakableBlock : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase breakableTile;
    public GameObject debrisPrefab;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            foreach (ContactPoint2D hit in collision.contacts)
            {
                // ปรับให้โหม่งง่ายขึ้น: เช็กแค่ว่าจุดชนอยู่สูงกว่าตัวละคร
                if (hit.point.y > collision.transform.position.y)
                {
                    // ดันจุดเช็กเข้าไปในเนื้อบล็อกให้ลึกขึ้น (0.5f) เพื่อความชัวร์
                    Vector3 checkPos = hit.point + (Vector2.up * 0.5f);
                    Vector3Int cellPosition = tilemap.WorldToCell(checkPos);

                    TileBase hitTile = tilemap.GetTile(cellPosition);

                    // บรรทัดนี้จะบอกคุณใน Console ว่าโหม่งโดนอะไร
                    Debug.Log("โหม่งที่พิกัด: " + cellPosition + " เจอ Tile: " + (hitTile ? hitTile.name : "null"));

                    if (hitTile == breakableTile)
                    {
                        BreakBlock(cellPosition);
                        return;
                    }
                }
            }
        }
    }

    void BreakBlock(Vector3Int position)
    {
        tilemap.SetTile(position, null);
        if (debrisPrefab != null)
        {
            Instantiate(debrisPrefab, tilemap.GetCellCenterWorld(position), Quaternion.identity);
        }
    }
}