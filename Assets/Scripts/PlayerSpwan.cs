using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerSpwan : MonoBehaviour
{
    public Tilemap tilemap; // Ÿ�ϸ� ����
    public GameObject playerPrefab; // �÷��̾� ������
    public TileBase groundTile; // �� Ÿ��
    public TileBase waterTile; // �ٴ� Ÿ��

    private List<Vector3Int> groundTiles = new List<Vector3Int>(); // �� Ÿ�� ��ġ ����

    public void Spwan()
    {
        if (tilemap == null || playerPrefab == null || groundTile == null)
        {
            Debug.LogError("Tilemap, PlayerPrefab �Ǵ� GroundTile�� �������� �ʾҽ��ϴ�.");
            return;
        }

        // �� Ÿ�� �� ������ ��ġ ã��
        Vector3Int spawnPosition = FindRandomGroundTile();
        if (spawnPosition == Vector3Int.zero)
        {
            Debug.LogError("Ÿ�ϸʿ� �� Ÿ���� �����ϴ�.");
            return;
        }

        // ���� ��ǥ�� ��ȯ �� �÷��̾� ����
        Vector3 worldPosition = tilemap.CellToWorld(spawnPosition) + new Vector3(0.5f, 0.5f, 0); // Ÿ�� �߽� ����
        Instantiate(playerPrefab, worldPosition, Quaternion.identity);
    }
    private Vector3Int FindRandomGroundTile()
    {
        // �� Ÿ�� ��ġ ����
        List<Vector3Int> groundTiles = new List<Vector3Int>();
        BoundsInt bounds = tilemap.cellBounds;

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                TileBase tile = tilemap.GetTile(position);

                // �� Ÿ���̸� ����Ʈ�� �߰�
                if (tile == groundTile)
                {
                    groundTiles.Add(position);
                }
            }
        }

        // �� Ÿ�� �� �������� ����
        if (groundTiles.Count > 0)
        {
            Vector3Int randomGround = groundTiles[Random.Range(0, groundTiles.Count)];
            Debug.Log($"�������� ���õ� �� ��ġ: {randomGround}");
            return randomGround;
        }

        // �� Ÿ���� ������ �⺻�� ��ȯ
        Debug.LogError("�� Ÿ���� �����ϴ�.");
        return Vector3Int.zero;
    }
}
