using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmSystem : MonoBehaviour
{
    public FarmSlot[] farmSlots; // 농장 슬롯 배열
    public ItemInventory inventory; // 플레이어 인벤토리

    public void PlantSeed(int slotIndex, ItemObject seed)
    {
        if (slotIndex >= 0 && slotIndex < farmSlots.Length)
        {
            farmSlots[slotIndex].PlantSeed(seed);
        }
    }

    public void WaterSlot(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < farmSlots.Length)
        {
            farmSlots[slotIndex].WaterPlant();
        }
    }

    public void HarvestCrops(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < farmSlots.Length)
        {
            ItemObject harvestedCrops = farmSlots[slotIndex].HarvestCrops();
            if (harvestedCrops != null)
            {
                inventory.items.Add(harvestedCrops); // 인벤토리에 작물 추가
                Debug.Log($"{harvestedCrops.Item_Name}을(를) 수확하여 인벤토리에 추가했습니다!");
            }
            else
            {
                Debug.Log("수확할 작물이 없습니다.");
            }
        }
    }
}
