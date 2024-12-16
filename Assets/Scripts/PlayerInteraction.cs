using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private Iitem currentItem;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (currentItem == null) return;

            // 아이템 소비 및 currentItem 초기화
            currentItem.OnConsume(gameObject);
            currentItem = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Iitem>(out var item))
        {
            currentItem = item;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Iitem>(out var item))
        {
            if (currentItem == item)
            {
                currentItem = null;
            }
        }
    }
}
