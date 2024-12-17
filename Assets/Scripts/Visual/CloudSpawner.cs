using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    public GameObject objectPrefab; // 생성할 오브젝트 프리팹
    public float spawnInterval = 5f; // 오브젝트 생성 간격 (초)
    public float spawnYMin = -5f; // 오브젝트 생성 최소 Y 위치
    public float spawnYMax = 5f; // 오브젝트 생성 최대 Y 위치
    public MapManager mapManager; // 맵 매니저

    private Bounds mapBounds;

    private void Update()
    {
        // 맵의 경계 가져오기
        mapBounds = mapManager.GetMapBounds();

        // 코루틴 시작
        StartCoroutine(SpawnObjects());
    }

    private IEnumerator SpawnObjects()
    {
        while (true)
        {
            // 오브젝트 생성
            SpawnObject();

            // spawnInterval 초 대기
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnObject()
    {
        // 오브젝트의 생성 Y 위치 설정
        float spawnY = Random.Range(spawnYMin, spawnYMax);

        // 왼쪽 또는 오른쪽에서 무작위로 생성
        bool spawnLeft = Random.value > 0.5f;
        Vector3 spawnPosition;
        int direction;

        if (spawnLeft)
        {
            // 맵 왼쪽 밖에서 생성
            spawnPosition = new Vector3(mapBounds.min.x - 1, spawnY, 0);
            direction = 1; // 오른쪽으로 이동
        }
        else
        {
            // 맵 오른쪽 밖에서 생성
            spawnPosition = new Vector3(mapBounds.max.x + 1, spawnY, 0);
            direction = -1; // 왼쪽으로 이동
        }

        // 오브젝트 생성
        GameObject obj = Instantiate(objectPrefab, spawnPosition, Quaternion.identity);

        // 이동 방향 설정
        obj.GetComponent<CloudMove>().Initialize(direction, mapBounds);
    }
}
