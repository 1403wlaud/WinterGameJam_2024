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
            inventory.items.Add(item); // ���� ������ ������ �߰�
            inventory.slotManager.AddItemToInventory(item); // ���� UI �ݿ�
            Destroy(gameObject); // ���忡�� ������ ����
        }
    }
}
