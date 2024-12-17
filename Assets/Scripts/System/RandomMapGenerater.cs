using UnityEngine.Tilemaps;
using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;

public class RandomMapGenerater : MonoBehaviour
{
    [Header("타일맵 관련")]
    [SerializeField] private Tilemap tileMap;
    [SerializeField] private TileBase[] waterTiles;
    [SerializeField] private TileBase[] sandTiles;
    [SerializeField] private TileBase[] lawnTiles;
    [SerializeField] private TileBase[] gressTiles;
    [SerializeField] private TileBase[] forestTiles;

    [Header("풀 오브젝트 관련")]
    [SerializeField] private GameObject[] grassObjects; // 풀 오브젝트 배열
    [SerializeField] private int maxGrassObjects = 100; // 최대 풀 오브젝트 수

    [Header("나무 오브젝트 관련")]
    [SerializeField] private GameObject[] treeObjects; // 나무 오브젝트 배열
    [SerializeField] private int maxTreeObjects = 50; // 최대 나무 오브젝트 수

    [Header("새로운 오브젝트 관련")]
    [SerializeField] private GameObject[] newObjects; // 새로운 오브젝트 배열
    [SerializeField] private int maxNewObjects = 75; // 최대 새로운 오브젝트 수

    [Header("값 관련")]
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

                // gressTiles와 forestTiles에 해당하는 타일 위치 저장
                if (IsTileInArray(currentTile, gressTiles) || IsTileInArray(currentTile, forestTiles))
                {
                    grassPositions.Add(point);
                }

                // forestTiles에 해당하는 타일 위치 저장
                if (IsTileInArray(currentTile, forestTiles))
                {
                    treePositions.Add(point);
                }

                // lawnTiles에 해당하는 타일 위치 저장
                if (IsTileInArray(currentTile, lawnTiles))
                {
                    newObjectPositions.Add(point);
                }
            }
        }

        // gressTiles와 forestTiles 위치에 풀 오브젝트 생성
        SpawnObjectsAtRandomPositions(grassObjects, grassPositions, maxGrassObjects);

        // forestTiles 위치에 나무 오브젝트 생성
        SpawnObjectsAtRandomPositions(treeObjects, treePositions, maxTreeObjects);

        // lawnTiles 위치에 새로운 오브젝트 생성
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
        // 입력된 오브젝트 배열이나 위치 리스트가 비어있는지 확인
        if (objects == null || objects.Length == 0 || positions == null || positions.Count == 0)
            return;

        // 생성할 오브젝트 수를 위치 리스트와 최대값 중 작은 값으로 설정
        int objectsToSpawn = Mathf.Min(maxObjects, positions.Count);

        // 무작위로 위치를 선택하여 오브젝트 생성
        for (int i = 0; i < objectsToSpawn; i++)
        {
            // 위치 리스트에서 무작위 인덱스 선택
            int randomIndex = Random.Range(0, positions.Count);
            Vector3Int selectedPosition = positions[randomIndex];

            // 선택된 위치를 리스트에서 제거하여 중복 방지
            positions.RemoveAt(randomIndex);

            // 타일맵의 셀 위치를 월드 위치로 변환
            Vector3 worldPosition = tileMap.CellToWorld(selectedPosition);

            // 오브젝트 배열에서 무작위로 프리팹 선택
            GameObject prefab = objects[Random.Range(0, objects.Length)];

            // 오브젝트 생성
            Instantiate(prefab, worldPosition, Quaternion.identity,tileMap.transform);
        }
    }
}