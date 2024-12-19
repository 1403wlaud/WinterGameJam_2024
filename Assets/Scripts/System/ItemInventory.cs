using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInventory : MonoBehaviour
{
    public List<ItemObject> items; // ������ �����͸� �����ϴ� ����Ʈ
    public SlotManager slotManager; // ���� �Ŵ���
    public int Money = 0;

    private void Update()
    {
        SyncSlotsWithInventory();
    }

    /// <summary>
    /// �κ��丮�� ������ ����ȭ
    /// </summary>
    public void SyncSlotsWithInventory()
    {
        if (slotManager == null || slotManager.slots == null)
            return;

        var slots = slotManager.slots;

        // ��� ���� �ʱ�ȭ
        foreach (var slot in slots)
        {
            slot.currentSlotItem = null;
            slot.stackCount = 0;
        }

        // �κ��丮�� �������� ���Կ� �ݿ�
        foreach (var item in items)
        {
            slotManager.AddItemToInventory(item);
        }

        Debug.Log("�κ��丮�� ���� ����ȭ �Ϸ�");
    }
}
