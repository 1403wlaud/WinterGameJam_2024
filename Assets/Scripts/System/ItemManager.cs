using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour,Iitem
{
    [SerializeField] private Item item;
    void Iitem.OnConsume(GameObject pUser)
    {
        pUser.GetComponent<ItemInventory>().items.Add(item);
    }
}
