using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInventory : MonoBehaviour
{
    public List<Item> items;

    private void Update()
    {
        foreach (var item in items)
        {
            if (item == null) return;
            Debug.Log(item.name);
        }
    }
}
