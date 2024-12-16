using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerSpwan : MonoBehaviour
{
    public Tilemap tilemap;
    public GameObject playerPrefab;
    public GameObject HousePrefab;
    public TileBase groundTile;
    public TileBase waterTile;

    private List<Vector3Int> groundTiles = new List<Vector3Int>();

    public void Spwan()
    {
        if (tilemap == null || playerPrefab == null || groundTile == null)
        {
            Debug.LogError("Tilemap, PlayerPrefab �Ǵ� GroundTile�� �������� �ʾҽ��ϴ�.");
            return;
        }

        Vector3Int spawnPosition = FindRandomGroundTile();
        if (spawnPosition == Vector3Int.zero)
        {
            Debug.LogError("Ÿ�ϸʿ� �� Ÿ���� �����ϴ�.");
            return;
        }


        Vector3 worldPosition = tilemap.CellToWorld(spawnPosition) + new Vector3(0.5f, 0.5f, 0);
        Instantiate(playerPrefab, worldPosition, Quaternion.identity);
        worldPosition.x += 10;
        worldPosition.y += 10;
        Instantiate(HousePrefab, worldPosition, Quaternion.identity);
    }
    private Vector3Int FindRandomGroundTile()
    {
        List<Vector3Int> groundTiles = new List<Vector3Int>();
        BoundsInt bounds = tilemap.cellBounds;

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                TileBase tile = tilemap.GetTile(position);

                if (tile == groundTile)
                {
                    groundTiles.Add(position);
                }
            }
        }

        if (groundTiles.Count > 0)
        {
            Vector3Int randomGround = groundTiles[Random.Range(0, groundTiles.Count)];
            Debug.Log($"�������� ���õ� �� ��ġ: {randomGround}");
            return randomGround;
        }

        Debug.LogError("�� Ÿ���� �����ϴ�.");
        return Vector3Int.zero;
    }
}
