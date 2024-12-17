using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public FarmSystem farmSystem;

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.P)) // 씨앗 심기 테스트
        //{
        //    farmSystem.PlantSeed(0, GetComponent<ItemInventory>().items[0]);
        //}

        //if (Input.GetKeyDown(KeyCode.W)) // 물 주기
        //{
        //    farmSystem.WaterSlot(0);
        //}

        //if (Input.GetKeyDown(KeyCode.H)) // 수확하기
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
