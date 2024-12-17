using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CloudMove : MonoBehaviour
{
    public float speed = 1f; // ������Ʈ�� �̵� �ӵ�
    private int direction = 1; // �̵� ���� (1: ������, -1: ����)
    private Bounds mapBounds; // ���� ���

    private void Update()
    {
        // ������ �������� �̵�
        transform.Translate(Vector3.right * speed * direction * Time.deltaTime);

        // �� ������ ������ ������ ����
        if (IsOutOfMap())
        {
            Destroy(gameObject);
        }
    }

    public void Initialize(int dir, Bounds bounds)
    {
        direction = dir;
        mapBounds = bounds;
    }

    private bool IsOutOfMap()
    {
        // ������Ʈ�� ���� ��ġ
        float posX = transform.position.x;

        // ���� ��踦 ������� Ȯ��
        return posX < mapBounds.min.x || posX > mapBounds.max.x;
    }
}
