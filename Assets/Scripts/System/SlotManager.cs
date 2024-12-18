using System.Collections.Generic;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
    public ItemInventory inventory; 
    public Slot[] slots;

    public void AddItemToInventory(ItemObject newItem)
    {
        if (inventory == null || slots == null) return;

        // �̹� �����ϴ� ���� ã��
        Slot existingSlot = FindSlotWithItem(newItem);
        if (existingSlot != null)
        {
            if (existingSlot.CanAddToStack())
            {
                existingSlot.AddToStack();
                return;
            }
        }

        // �� ���� ã��
        int emptySlotIndex = FindEmptySlot();
        if (emptySlotIndex != -1)
        {
            AssignItemToSlot(emptySlotIndex, newItem);
        }
    }

    private Slot FindSlotWithItem(Item item)
    {
        foreach (Slot slot in slots)
        {
            if (slot.IsSameItem(item) && slot.CanAddToStack())
            {
                return slot;
            }
        }
        return null;
    }

    private int FindEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].currentSlotItem == null)
            {
                return i;
            }
        }
        return -1;
    }

    private void AssignItemToSlot(int slotIndex, Item item)
    {
        slots[slotIndex].currentSlotItem = item;
        slots[slotIndex].stackCount = 1; // ���� �߰��� ��� ���� 1
    }
}
