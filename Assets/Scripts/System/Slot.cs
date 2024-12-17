using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item currentSlotItem; // ���� ���� ������
    public int stackCount = 0;   // �ش� ������ ������ ����
    public int maxStack = 99;   // �ִ� ���� ����

    private GameObject child;

    private void Start()
    {
        child = transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        if (currentSlotItem != null)
        {
            child.SetActive(true);
            child.GetComponent<Image>().sprite = currentSlotItem.Item_Image;

            // ���� ������ ǥ�� (��: �ؽ�Ʈ)
            Text stackText = child.GetComponentInChildren<Text>();
            if (stackText != null)
            {
                stackText.text = stackCount > 1 ? stackCount.ToString() : ""; // 2�� �̻��̸� ���� ǥ��
            }
        }
        else
        {
            child.SetActive(false);
        }
    }

    public bool CanAddToStack()
    {
        return stackCount < maxStack;
    }

    public void AddToStack()
    {
        if (CanAddToStack())
        {
            stackCount++;
        }
    }
    public bool IsSameItem(Item item)
    {
        return currentSlotItem == item;
    }
}
