using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInventory : MonoBehaviour
{
    public List<ItemObject> items; // 아이템 데이터를 저장하는 리스트
    public SlotManager slotManager; // 슬롯 매니저
    public int Money = 0;

    private void Update()
    {
        SyncSlotsWithInventory();
    }

    /// <summary>
    /// 인벤토리와 슬롯을 동기화
    /// </summary>
    public void SyncSlotsWithInventory()
    {
        if (slotManager == null || slotManager.slots == null)
            return;

        var slots = slotManager.slots;

        // 모든 슬롯 초기화
        foreach (var slot in slots)
        {
            slot.currentSlotItem = null;
            slot.stackCount = 0;
        }

        // 인벤토리의 아이템을 슬롯에 반영
        foreach (var item in items)
        {
            slotManager.AddItemToInventory(item);
        }

        Debug.Log("인벤토리와 슬롯 동기화 완료");
    }
}
