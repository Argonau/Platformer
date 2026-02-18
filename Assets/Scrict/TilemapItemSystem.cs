using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapItemBlock : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase questionTile;
    public TileBase emptyTile;
    public GameObject itemPrefab;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            foreach (ContactPoint2D hit in collision.contacts)
            {
                if (hit.point.y > collision.transform.position.y)
                {
                    Vector3 checkPos = hit.point + (Vector2.up * 0.5f);
                    Vector3Int cellPosition = tilemap.WorldToCell(checkPos);

                    TileBase hitTile = tilemap.GetTile(cellPosition);

                    if (hitTile == questionTile)
                    {
                        HitBlock(cellPosition);
                        return;
                    }
                }
            }
        }
    }

    void HitBlock(Vector3Int position)
    {
        tilemap.SetTile(position, emptyTile);
        Vector3 spawnPos = tilemap.GetCellCenterWorld(position) + (Vector3.up * 1.5f);
        if (itemPrefab != null)
        {
            // ดันแกน Z มาข้างหน้าเพื่อให้เห็นไอเทมชัดเจน
            Instantiate(itemPrefab, new Vector3(spawnPos.x, spawnPos.y, -2f), Quaternion.identity);
        }
    }
}