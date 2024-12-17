using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    public GameObject objectPrefab; // ������ ������Ʈ ������
    public float spawnInterval = 5f; // ������Ʈ ���� ���� (��)
    public float spawnYMin = -5f; // ������Ʈ ���� �ּ� Y ��ġ
    public float spawnYMax = 5f; // ������Ʈ ���� �ִ� Y ��ġ
    public MapManager mapManager; // �� �Ŵ���

    private Bounds mapBounds;

    private void Update()
    {
        // ���� ��� ��������
        mapBounds = mapManager.GetMapBounds();

        // �ڷ�ƾ ����
        StartCoroutine(SpawnObjects());
    }

    private IEnumerator SpawnObjects()
    {
        while (true)
        {
            // ������Ʈ ����
            SpawnObject();

            // spawnInterval �� ���
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnObject()
    {
        // ������Ʈ�� ���� Y ��ġ ����
        float spawnY = Random.Range(spawnYMin, spawnYMax);

        // ���� �Ǵ� �����ʿ��� �������� ����
        bool spawnLeft = Random.value > 0.5f;
        Vector3 spawnPosition;
        int direction;

        if (spawnLeft)
        {
            // �� ���� �ۿ��� ����
            spawnPosition = new Vector3(mapBounds.min.x - 1, spawnY, 0);
            direction = 1; // ���������� �̵�
        }
        else
        {
            // �� ������ �ۿ��� ����
            spawnPosition = new Vector3(mapBounds.max.x + 1, spawnY, 0);
            direction = -1; // �������� �̵�
        }

        // ������Ʈ ����
        GameObject obj = Instantiate(objectPrefab, spawnPosition, Quaternion.identity);

        // �̵� ���� ����
        obj.GetComponent<CloudMove>().Initialize(direction, mapBounds);
    }
}
