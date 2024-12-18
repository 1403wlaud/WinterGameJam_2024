using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class PlayerSpwan : MonoBehaviour
{
    public Tilemap tilemap;
    public GameObject playerPrefab;
    public GameObject housePrefab;
    public TileBase groundTile;
    public TileBase waterTile;

    private List<Vector3Int> groundTiles = new List<Vector3Int>();

    [HideInInspector]
    public GameObject spawnedHouse; // 생성된 집을 참조하기 위한 변수

    public void Spawn()
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

        // 타일맵의 셀 크기 가져오기
        Vector3 cellSize = tilemap.cellSize;

        // 셀 위치를 월드 위치로 변환하고 셀 크기의 절반을 더하여 중앙에 배치
        Vector3 worldPosition = tilemap.CellToWorld(spawnPosition) + new Vector3(cellSize.x * 0.5f, cellSize.y * 0.5f, 0);
        Instantiate(playerPrefab, worldPosition, Quaternion.identity);

        // 집은 플레이어 스폰 위치에서 약간 떨어진 위치에 생성
        Vector3 housePosition = worldPosition + new Vector3(0, 4, 0);
        spawnedHouse=Instantiate(housePrefab, housePosition, Quaternion.identity);
    }

    private Vector3Int FindRandomGroundTile()
    {
        List<Vector3Int> groundTiles = new List<Vector3Int>();

        // 타일맵의 셀 경계 가져오기
        BoundsInt bounds = tilemap.cellBounds;

        // 스폰 가능한 범위 설정 (예: 중앙 50% 영역)
        int xRangeMin = bounds.xMin + (bounds.xMax - bounds.xMin) / 4;
        int xRangeMax = bounds.xMax - (bounds.xMax - bounds.xMin) / 4;
        int yRangeMin = bounds.yMin + (bounds.yMax - bounds.yMin) / 4;
        int yRangeMax = bounds.yMax - (bounds.yMax - bounds.yMin) / 4;

        for (int x = xRangeMin; x <= xRangeMax; x++)
        {
            for (int y = yRangeMin; y <= yRangeMax; y++)
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
