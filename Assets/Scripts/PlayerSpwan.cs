using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerSpwan : MonoBehaviour
{
    public Tilemap tilemap;
    public GameObject playerPrefab;
    public GameObject housePrefab;
    public TileBase groundTile;
    public TileBase waterTile;

    private List<Vector3Int> groundTiles = new List<Vector3Int>();

    public void Spwan()
    {
        if (tilemap == null || playerPrefab == null || groundTile == null)
        {
            return;
        }

        Vector3Int spawnPosition = FindRandomGroundTile();
        if (spawnPosition == Vector3Int.zero)
        {
            return;
        }

        Vector3 worldPosition = tilemap.CellToWorld(spawnPosition) + new Vector3(0.5f, 0.5f, 0);
        Instantiate(playerPrefab, worldPosition, Quaternion.identity);

        // 집은 플레이어 스폰 위치에서 약간 떨어진 위치에 생성
        worldPosition.x += 10;
        worldPosition.y += 10;
        Instantiate(housePrefab, worldPosition, Quaternion.identity);
    }

    private Vector3Int FindRandomGroundTile()
    {
        List<Vector3Int> groundTiles = new List<Vector3Int>();

        // 지정된 좌표 범위
        int xMin = 147;
        int xMax = 580;
        int yMin = 84;
        int yMax = 600;

        for (int x = xMin; x <= xMax; x++)
        {
            for (int y = yMin; y <= yMax; y++)
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
            return randomGround;
        }

        return Vector3Int.zero; // GroundTile이 없으면 기본값 반환
    }
}
