using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour, Iitem
{
    [SerializeField] private ItemObject item;

    void Iitem.OnConsume(GameObject pUser)
    {
        var inventory = pUser.GetComponent<ItemInventory>();
        if (inventory != null)
        {
            inventory.items.Add(item); // 실제 아이템 데이터 추가
            inventory.slotManager.AddItemToInventory(item); // 슬롯 UI 반영
            Destroy(gameObject); // 월드에서 아이템 삭제
        }
    }
}
