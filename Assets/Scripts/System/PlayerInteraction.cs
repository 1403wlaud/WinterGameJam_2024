using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public FarmSystem farmSystem;

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.P)) // ���� �ɱ� �׽�Ʈ
        //{
        //    farmSystem.PlantSeed(0, GetComponent<ItemInventory>().items[0]);
        //}

        //if (Input.GetKeyDown(KeyCode.W)) // �� �ֱ�
        //{
        //    farmSystem.WaterSlot(0);
        //}

        //if (Input.GetKeyDown(KeyCode.H)) // ��Ȯ�ϱ�
        //{
        //    farmSystem.HarvestCrops(0);
        //}
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.TryGetComponent<Iitem>(out var item))
    //    {
    //        currentItem = item;
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.TryGetComponent<Iitem>(out var item))
    //    {
    //        if (currentItem == item)
    //        {
    //            currentItem = null;
    //        }
    //    }
    //}
}
