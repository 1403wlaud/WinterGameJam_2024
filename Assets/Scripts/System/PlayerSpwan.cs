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
    public GameObject spawnedHouse; // ������ ���� �����ϱ� ���� ����

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

        // Ÿ�ϸ��� �� ũ�� ��������
        Vector3 cellSize = tilemap.cellSize;

        // �� ��ġ�� ���� ��ġ�� ��ȯ�ϰ� �� ũ���� ������ ���Ͽ� �߾ӿ� ��ġ
        Vector3 worldPosition = tilemap.CellToWorld(spawnPosition) + new Vector3(cellSize.x * 0.5f, cellSize.y * 0.5f, 0);
        Instantiate(playerPrefab, worldPosition, Quaternion.identity);

        // ���� �÷��̾� ���� ��ġ���� �ణ ������ ��ġ�� ����
        Vector3 housePosition = worldPosition + new Vector3(0, 4, 0);
        spawnedHouse=Instantiate(housePrefab, housePosition, Quaternion.identity);
    }

    private Vector3Int FindRandomGroundTile()
    {
        List<Vector3Int> groundTiles = new List<Vector3Int>();

        // Ÿ�ϸ��� �� ��� ��������
        BoundsInt bounds = tilemap.cellBounds;

        // ���� ������ ���� ���� (��: �߾� 50% ����)
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

        return Vector3Int.zero; // GroundTile�� ������ �⺻�� ��ȯ
    }
}
