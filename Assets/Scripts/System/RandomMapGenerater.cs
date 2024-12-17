using UnityEngine.Tilemaps;
using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;

public class RandomMapGenerater : MonoBehaviour
{
    [Header("Ÿ�ϸ� ����")]
    [SerializeField] private Tilemap tileMap;
    [SerializeField] private TileBase[] waterTiles;
    [SerializeField] private TileBase[] sandTiles;
    [SerializeField] private TileBase[] lawnTiles;
    [SerializeField] private TileBase[] gressTiles;
    [SerializeField] private TileBase[] forestTiles;

    [Header("Ǯ ������Ʈ ����")]
    [SerializeField] private GameObject[] grassObjects; // Ǯ ������Ʈ �迭
    [SerializeField] private int maxGrassObjects = 100; // �ִ� Ǯ ������Ʈ ��

    [Header("���� ������Ʈ ����")]
    [SerializeField] private GameObject[] treeObjects; // ���� ������Ʈ �迭
    [SerializeField] private int maxTreeObjects = 50; // �ִ� ���� ������Ʈ ��

    [Header("���ο� ������Ʈ ����")]
    [SerializeField] private GameObject[] newObjects; // ���ο� ������Ʈ �迭
    [SerializeField] private int maxNewObjects = 75; // �ִ� ���ο� ������Ʈ ��

    [Header("�� ����")]
    [SerializeField] private float mapScale = 0.01f;
    [SerializeField] private int mapSize;
    [SerializeField] private int octaves;

    public PlayerSpwan playerSpwan;

    private float seed;

    private async void Awake()
    {
        seed = Random.Range(0, 10000f);
        var noiseArr = await Task.Run(GenerateNoise);
        SettingTileMap(noiseArr);
    }

    private float[,] GenerateNoise()
    {
        float[,] noiseArr = new float[mapSize, mapSize];
        float min = float.MaxValue, max = float.MinValue;

        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                float lacunarity = 2.0f;
                float gain = 0.5f;
                float amplitude = 0.5f;
                float frequency = 1f;

                for (int i = 0; i < octaves; i++)
                {
                    noiseArr[x, y] += amplitude * (Mathf.PerlinNoise(
                        seed + (x * mapScale * frequency),
                        seed + (y * mapScale * frequency)) * 2 - 1);

                    frequency *= lacunarity;
                    amplitude *= gain;
                }

                if (noiseArr[x, y] < min) min = noiseArr[x, y];
                else if (noiseArr[x, y] > max) max = noiseArr[x, y];
            }
        }

        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                noiseArr[x, y] = Mathf.InverseLerp(min, max, noiseArr[x, y]);
            }
        }
        return noiseArr;
    }

    private void SettingTileMap(float[,] noiseArr)
    {
        Vector3Int point = Vector3Int.zero;
        List<Vector3Int> grassPositions = new List<Vector3Int>();
        List<Vector3Int> treePositions = new List<Vector3Int>();
        List<Vector3Int> newObjectPositions = new List<Vector3Int>();

        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                point.Set(x, y, 0);
                TileBase currentTile = GetTileByHight(noiseArr[x, y]);
                tileMap.SetTile(point, currentTile);

                // gressTiles�� forestTiles�� �ش��ϴ� Ÿ�� ��ġ ����
                if (IsTileInArray(currentTile, gressTiles) || IsTileInArray(currentTile, forestTiles))
                {
                    grassPositions.Add(point);
                }

                // forestTiles�� �ش��ϴ� Ÿ�� ��ġ ����
                if (IsTileInArray(currentTile, forestTiles))
                {
                    treePositions.Add(point);
                }

                // lawnTiles�� �ش��ϴ� Ÿ�� ��ġ ����
                if (IsTileInArray(currentTile, lawnTiles))
                {
                    newObjectPositions.Add(point);
                }
            }
        }

        // gressTiles�� forestTiles ��ġ�� Ǯ ������Ʈ ����
        SpawnObjectsAtRandomPositions(grassObjects, grassPositions, maxGrassObjects);

        // forestTiles ��ġ�� ���� ������Ʈ ����
        SpawnObjectsAtRandomPositions(treeObjects, treePositions, maxTreeObjects);

        // lawnTiles ��ġ�� ���ο� ������Ʈ ����
        SpawnObjectsAtRandomPositions(newObjects, newObjectPositions, maxNewObjects);

        playerSpwan.Spawn();
    }

    private TileBase GetTileByHight(float hight)
    {
        switch (hight)
        {
            case <= 0.35f: return GetRandomTile(waterTiles);
            case <= 0.42f: return GetRandomTile(sandTiles);
            case <= 0.50f: return GetRandomTile(lawnTiles);
            case <= 0.7f: return GetRandomTile(gressTiles);
            default: return GetRandomTile(forestTiles);
        }
    }

    private TileBase GetRandomTile(TileBase[] tiles)
    {
        if (tiles == null || tiles.Length == 0) return null;
        return tiles[Random.Range(0, tiles.Length)];
    }

    private bool IsTileInArray(TileBase tile, TileBase[] tileArray)
    {
        foreach (TileBase t in tileArray)
        {
            if (t == tile)
                return true;
        }
        return false;
    }


    private void SpawnObjectsAtRandomPositions(GameObject[] objects, List<Vector3Int> positions, int maxObjects)
    {
        // �Էµ� ������Ʈ �迭�̳� ��ġ ����Ʈ�� ����ִ��� Ȯ��
        if (objects == null || objects.Length == 0 || positions == null || positions.Count == 0)
            return;

        // ������ ������Ʈ ���� ��ġ ����Ʈ�� �ִ밪 �� ���� ������ ����
        int objectsToSpawn = Mathf.Min(maxObjects, positions.Count);

        // �������� ��ġ�� �����Ͽ� ������Ʈ ����
        for (int i = 0; i < objectsToSpawn; i++)
        {
            // ��ġ ����Ʈ���� ������ �ε��� ����
            int randomIndex = Random.Range(0, positions.Count);
            Vector3Int selectedPosition = positions[randomIndex];

            // ���õ� ��ġ�� ����Ʈ���� �����Ͽ� �ߺ� ����
            positions.RemoveAt(randomIndex);

            // Ÿ�ϸ��� �� ��ġ�� ���� ��ġ�� ��ȯ
            Vector3 worldPosition = tileMap.CellToWorld(selectedPosition);

            // ������Ʈ �迭���� �������� ������ ����
            GameObject prefab = objects[Random.Range(0, objects.Length)];

            // ������Ʈ ����
            Instantiate(prefab, worldPosition, Quaternion.identity,tileMap.transform);
        }
    }
}