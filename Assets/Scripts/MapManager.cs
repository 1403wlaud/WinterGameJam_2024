using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    public Tilemap tilemap;
    public Bounds mapSize;

    private void Start()
    {
        
    }
    private void Update()
    {
        mapSize = GetMapBounds();
        Debug.Log(mapSize);
    }
    public Bounds GetMapBounds()
    {
        // Ÿ�ϸ��� �� ��� ��������
        BoundsInt cellBounds = tilemap.cellBounds;

        // Ÿ�ϸ��� ���� ��� ���
        Vector3 min = tilemap.CellToWorld(cellBounds.min);
        Vector3 max = tilemap.CellToWorld(cellBounds.max);
        Bounds bounds = new Bounds();
        bounds.SetMinMax(min, max);

        return bounds;
    }
}
