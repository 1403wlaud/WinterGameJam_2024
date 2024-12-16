using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item currentSlotItem; // 현재 슬롯 아이템
    public int stackCount = 0;   // 해당 슬롯의 아이템 개수
    public int maxStack = 99;   // 최대 스택 개수

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

            // 스택 개수를 표시 (예: 텍스트)
            Text stackText = child.GetComponentInChildren<Text>();
            if (stackText != null)
            {
                stackText.text = stackCount > 1 ? stackCount.ToString() : ""; // 2개 이상이면 숫자 표시
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
