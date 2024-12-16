using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerSpwan : MonoBehaviour
{
    public Tilemap tilemap; // 타일맵 참조
    public GameObject playerPrefab; // 플레이어 프리팹
    public TileBase groundTile; // 땅 타일
    public TileBase waterTile; // 바다 타일

    private List<Vector3Int> groundTiles = new List<Vector3Int>(); // 땅 타일 위치 저장

    public void Spwan()
    {
        if (tilemap == null || playerPrefab == null || groundTile == null)
        {
            Debug.LogError("Tilemap, PlayerPrefab 또는 GroundTile이 설정되지 않았습니다.");
            return;
        }

        // 땅 타일 중 무작위 위치 찾기
        Vector3Int spawnPosition = FindRandomGroundTile();
        if (spawnPosition == Vector3Int.zero)
        {
            Debug.LogError("타일맵에 땅 타일이 없습니다.");
            return;
        }

        // 월드 좌표로 변환 후 플레이어 생성
        Vector3 worldPosition = tilemap.CellToWorld(spawnPosition) + new Vector3(0.5f, 0.5f, 0); // 타일 중심 보정
        Instantiate(playerPrefab, worldPosition, Quaternion.identity);
    }
    private Vector3Int FindRandomGroundTile()
    {
        // 땅 타일 위치 저장
        List<Vector3Int> groundTiles = new List<Vector3Int>();
        BoundsInt bounds = tilemap.cellBounds;

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                TileBase tile = tilemap.GetTile(position);

                // 땅 타일이면 리스트에 추가
                if (tile == groundTile)
                {
                    groundTiles.Add(position);
                }
            }
        }

        // 땅 타일 중 무작위로 선택
        if (groundTiles.Count > 0)
        {
            Vector3Int randomGround = groundTiles[Random.Range(0, groundTiles.Count)];
            Debug.Log($"랜덤으로 선택된 땅 위치: {randomGround}");
            return randomGround;
        }

        // 땅 타일이 없으면 기본값 반환
        Debug.LogError("땅 타일이 없습니다.");
        return Vector3Int.zero;
    }
}
