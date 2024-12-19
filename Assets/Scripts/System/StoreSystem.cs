using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreSystem : MonoBehaviour
{
    private GameObject Player;
    private ItemInventory inventory;

    public GameObject ItemSellPanel;
    public GameObject ItemBuyPanel;
    public GameObject BuySell_Panel;

    public GameObject BuyBtn;
    public GameObject SellBtn;

    public Image ItemImage;
    public Text SellText;
    public Text CalcText;
    public Text CountText;
    public Text ItemNameText;
    public Text[] InvenItemCountText;

    public ItemObject[] itemObjects; // 상점에서 판매 가능한 아이템 목록
    public ItemObject[] SeedItemObjects; // 상점에서 구매 가능한 씨앗 목록

    private int Count = 0; // 판매 또는 구매 개수
    private int SelectedItemIndex = -1; // 선택된 아이템의 인덱스

    private bool SellMode;
    private bool BuyMode;

    private void Start()
    {
        SellPanelBtnOnClick();
    }

    private void Update()
    {
        if (GameObject.Find("Player(Clone)") != null)
            Player = GameObject.Find("Player(Clone)").gameObject;

        if (Player != null)
            inventory = Player.GetComponent<ItemInventory>();

        UpdateUI();
        
    }

    public void SellPanelBtnOnClick()
    {
        SellMode = true;
        BuyMode = false;
        BuyBtn.SetActive(false);
        SellBtn.SetActive(true);
        BuySell_Panel.SetActive(false);
        ItemSellPanel.SetActive(true);
        ItemBuyPanel.SetActive(false);
        SelectedItemIndex = -1; // 선택 초기화
    }

    public void BuyPanelBtnOnClick()
    {
        BuyMode = true;
        SellMode = false;
        SellBtn.SetActive(false);
        BuyBtn.SetActive(true);
        BuySell_Panel.SetActive(false);
        ItemSellPanel.SetActive(false);
        ItemBuyPanel.SetActive(true);
        SelectedItemIndex = -1; // 선택 초기화
    }

    public void UpBtnOn()
    {
        if (SelectedItemIndex != -1)
        {
            Count++;
            UpdateCalculation();
        }
    }

    public void DownBtnOn()
    {
        if (Count > 0)
        {
            Count--;
            UpdateCalculation();
        }
    }

    public void SellBtnOnClick()
    {
        if (SelectedItemIndex == -1 || Count <= 0 || inventory == null) return;

        ItemObject selectedItem = itemObjects[SelectedItemIndex];
        int playerItemCount = GetItemCountInInventory(selectedItem);

        if (playerItemCount >= Count)
        {
            for (int i = 0; i < Count; i++)
            {
                inventory.items.Remove(selectedItem);
            }

            inventory.Money += selectedItem.sellingPrice * Count;
            Debug.Log($"판매 완료: {selectedItem.Item_Name} {Count}개를 판매했습니다. 현재 소지금: {inventory.Money} G");

            Count = 0;
            UpdateCalculation();
            inventory.SyncSlotsWithInventory();
        }
        else
        {
            Debug.LogWarning("판매하려는 아이템이 부족합니다.");
        }
    }

    public void BuyBtnOnClick()
    {
        if (SelectedItemIndex == -1 || Count <= 0 || inventory == null) return;

        ItemObject selectedItem = SeedItemObjects[SelectedItemIndex];
        int totalPrice = selectedItem.Item_Price * Count;

        if (inventory.Money >= totalPrice)
        {
            for (int i = 0; i < Count; i++)
            {
                inventory.items.Add(selectedItem);
            }

            inventory.Money -= totalPrice;
            Debug.Log($"구매 완료: {selectedItem.Item_Name} {Count}개를 구매했습니다. 남은 소지금: {inventory.Money} G");

            Count = 0;
            UpdateCalculation();
            inventory.SyncSlotsWithInventory();
        }
        else
        {
            Debug.LogWarning("소지금이 부족합니다.");
        }
    }

    private int GetItemCountInInventory(ItemObject item)
    {
        int count = 0;
        foreach (var playerItem in inventory.items)
        {
            if (playerItem == item)
            {
                count++;
            }
        }
        return count;
    }

    private void UpdateCalculation()
    {
        if (SelectedItemIndex != -1)
        {
            ItemObject selectedItem = ItemSellPanel.activeSelf ? 
                itemObjects[SelectedItemIndex] : SeedItemObjects[SelectedItemIndex];
            int totalPrice;
            if (SellMode)
            {
                totalPrice = selectedItem.sellingPrice * Count;
                SellText.text = selectedItem.sellingPrice.ToString();
                CalcText.text = $"{totalPrice} G";
            }
            if (BuyMode)
            {
                SellText.text = selectedItem.Item_Price.ToString();
                totalPrice = selectedItem.Item_Price * Count;
                CalcText.text = $"{totalPrice} G";
            }
            CountText.text = Count.ToString();

            if (selectedItem != null)
            {
                ItemNameText.text = selectedItem.Item_Example;
                ItemImage.sprite = selectedItem.Item_Image;
            }
        }
        else
        {
            CalcText.text = "0 G";
            CountText.text = "0";
        }
    }

    private void UpdateUI()
    {
        if (inventory == null) return;

        // 인벤토리 아이템 개수 업데이트
        for (int i = 0; i < itemObjects.Length; i++)
        {
            int count = GetItemCountInInventory(itemObjects[i]);
            InvenItemCountText[i].text = count.ToString();
        }
    }

    public void SelectItem(int index)
    {
        BuySell_Panel.SetActive(true);
        SelectedItemIndex = index;
        Count = 0;
        UpdateCalculation();
    }
}
