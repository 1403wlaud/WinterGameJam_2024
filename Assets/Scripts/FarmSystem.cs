using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmSystem : MonoBehaviour
{
    public FarmSlot[] farmSlots; // ���� ���� �迭
    public ItemInventory inventory; // �÷��̾� �κ��丮

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
                inventory.items.Add(harvestedCrops); // �κ��丮�� �۹� �߰�
                Debug.Log($"{harvestedCrops.Item_Name}��(��) ��Ȯ�Ͽ� �κ��丮�� �߰��߽��ϴ�!");
            }
            else
            {
                Debug.Log("��Ȯ�� �۹��� �����ϴ�.");
            }
        }
    }
}
