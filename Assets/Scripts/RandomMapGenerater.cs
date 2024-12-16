using UnityEngine.Tilemaps;
using UnityEngine;
using System.Threading.Tasks;

public class RandomMapGenerater : MonoBehaviour
{

    [Header("타일맵 관련")]
    [SerializeField] private Tilemap tileMap;
    [SerializeField] private TileBase water, sand, gress, forest;
    [Space]
    [Header("값 관련")]
    [SerializeField] private float mapScale = 0.01f;
    [SerializeField] private int mapSize;
    [SerializeField] private int octaves; //노이즈 중첩 횟수

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

                float lacunarity = 2.0f; //주파수 증가율
                float gain = 0.5f; //루프당 진폭 감소율

                float amplitude = 0.5f; //진폭
                float frequency = 1f; //주파수

                //저주파 노이즈와 고주파 노이즈를 중첩
                for (int i = 0; i < octaves; i++)
                {

                    noiseArr[x, y] += amplitude * (Mathf.PerlinNoise(
                        seed + (x * mapScale * frequency),
                        seed + (y * mapScale * frequency)) * 2 - 1); // -1 ~ 1

                    frequency *= lacunarity;
                    amplitude *= gain;

                }

                if (noiseArr[x, y] < min)
                {

                    min = noiseArr[x, y];

                }
                else if (noiseArr[x, y] > max)
                {

                    max = noiseArr[x, y];

                }


            }

        }


        //중첩된 노이즈값을 0~1로 변환
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

        for (int x = 0; x < mapSize; x++)
        {

            for (int y = 0; y < mapSize; y++)
            {

                point.Set(x, y, 0);
                tileMap.SetTile(point, GetTileByHight(noiseArr[x, y]));

            }

        }

    }

    private TileBase GetTileByHight(float hight)
    {

        switch (hight)
        {

            case <= 0.35f: return water;
            case <= 0.45f: return sand;
            case <= 0.6f: return gress;
            default: return forest;

        }

    }

}