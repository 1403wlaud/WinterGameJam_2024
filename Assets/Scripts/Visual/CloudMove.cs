using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CloudMove : MonoBehaviour
{
    public float speed = 1f; // 오브젝트의 이동 속도
    private int direction = 1; // 이동 방향 (1: 오른쪽, -1: 왼쪽)
    private Bounds mapBounds; // 맵의 경계

    private void Update()
    {
        // 설정된 방향으로 이동
        transform.Translate(Vector3.right * speed * direction * Time.deltaTime);

        // 맵 밖으로 완전히 나가면 제거
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
        // 오브젝트의 현재 위치
        float posX = transform.position.x;

        // 맵의 경계를 벗어났는지 확인
        return posX < mapBounds.min.x || posX > mapBounds.max.x;
    }
}
