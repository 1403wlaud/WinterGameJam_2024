using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FarmSystem : MonoBehaviour
{
    [Header("농장 슬롯 관리")]
    public FarmSlot[] farmSlots; // 농장 슬롯 배열
    [Header("플레이어 인벤토리")]
    public ItemInventory inventory; // 플레이어 인벤토리
    [Header("선택된 씨앗")]
    public ItemObject selectedSeed; // 현재 선택된 씨앗
    [Header("물 양동이 버튼")]
    public Button waterButton; // 물 양동이 버튼
    private bool isWateringMode = false; // 물 주기 모드 여부

    [Header("씨앗 버튼 관리")]
    public Button[] seedButtons; // 씨앗 버튼 배열
    public ItemObject[] seedItems; // 각 씨앗 버튼에 대응하는 씨앗 아이템
    public Text[] seedCountTexts; // 씨앗 수를 표시할 텍스트 배열

    private Button selectedButton; // 선택된 버튼 저장

    private void Start()
    {
        SetupSeedButtons();
        SetupSlotButtons();

        // 물 양동이 버튼 이벤트 설정
        waterButton.onClick.RemoveAllListeners();
        waterButton.onClick.AddListener(ToggleWateringMode);
        AddOutlineComponent(waterButton);
    }
    private void Update()
    {
        UpdateSeedCounts(); // 씨앗 수를 실시간으로 업데이트
    }

    private void SetupSeedButtons()
    {
        for (int i = 0; i < seedButtons.Length; i++)
        {
            int index = i; // 로컬 변수로 캡처
            AddOutlineComponent(seedButtons[index]);

            // 기존 이벤트 제거
            seedButtons[index].onClick.RemoveAllListeners();

            // 클릭 이벤트 추가
            seedButtons[index].onClick.AddListener(() => OnSeedButtonClicked(index));
        }
    }

    private void UpdateSeedCounts()
    {
        for (int i = 0; i < seedItems.Length; i++)
        {
            int count = GetSeedCount(seedItems[i]); // 인벤토리에서 씨앗 수 확인
            seedCountTexts[i].text = count > 0 ? count.ToString() : "0"; // Text에 개수 표시
        }
    }
    private int GetSeedCount(ItemObject seed)
    {
        int count = 0;
        foreach (var item in inventory.items)
        {
            if (item.Item_Name == seed.Item_Name)
            {
                count++;
            }
        }
        return count;
    }

    private void SetupSlotButtons()
    {
        for (int i = 0; i < farmSlots.Length; i++)
        {
            int slotIndex = i;
            farmSlots[i].PlantBtn.onClick.AddListener(()
                => OnSlotButtonClicked(slotIndex));
        }
    }

    /// <summary>
    /// 물 양동이 모드 토글
    /// </summary>
    private void ToggleWateringMode()
    {
        if (isWateringMode)
        {
            isWateringMode = false;
            SetOutline(waterButton, false);
        }
        else
        {
            isWateringMode = true;
            selectedSeed = null;
            if (selectedButton != null)
            {
                SetOutline(selectedButton, false);
                selectedButton = null;
            }
            SetOutline(waterButton, true);
        }
    }

    /// <summary>
    /// 씨앗 선택 토글
    /// </summary>
    private void OnSeedButtonClicked(int index)
    {
        Button clickedButton = seedButtons[index];

        if (selectedButton == clickedButton) // 이미 선택된 버튼 클릭 시 해제
        {
            selectedSeed = null;
            SetOutline(clickedButton, false);
            selectedButton = null;
        }
        else // 새로운 버튼 선택
        {
            selectedSeed = seedItems[index];
            isWateringMode = false;
            SetOutline(clickedButton, true);
            ClearAllOtherOutlines(clickedButton);
            selectedButton = clickedButton; // 현재 선택된 버튼 업데이트
        }
    }


    private bool InventoryHasSeed(ItemObject seed)
    {
        return GetSeedCount(seed) > 0;
    }

    private void AddOutlineComponent(Button button)
    {
        if (button.GetComponent<Outline>() == null)
        {
            Outline outline = button.gameObject.AddComponent<Outline>();
            outline.enabled = false; // 초기에는 비활성화
            outline.effectColor = Color.yellow; // 강조 색상
            outline.effectDistance = new Vector2(5, 5); // 강조 효과 거리
        }
    }

    /// <summary>
    /// 씨앗을 인벤토리에서 하나 소모
    /// </summary>
    private bool ConsumeSeed(ItemObject seed)
    {
        for (int i = 0; i < inventory.items.Count; i++)
        {
            if (inventory.items[i].Item_Name == seed.Item_Name) // 이름으로 비교
            {
                inventory.items.RemoveAt(i); // 아이템 제거
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 특정 버튼의 Outline 설정
    /// </summary>
    private void SetOutline(Button button, bool state)
    {
        Outline outline = button.GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = state;
        }
    }

    /// <summary>
    /// 다른 버튼들의 Outline 해제
    /// </summary>
    private void ClearAllOtherOutlines(Button activeButton)
    {
        foreach (var button in seedButtons)
        {
            if (button != activeButton)
                SetOutline(button, false);
        }

        if (waterButton != activeButton)
            SetOutline(waterButton, false);
    }

    private void OnSlotButtonClicked(int slotIndex)
    {
        if (isWateringMode)
        {
            farmSlots[slotIndex].WaterPlant();
        }
        else if (selectedSeed != null && farmSlots[slotIndex].currentSeed == null)
        {
            if (ConsumeSeed(selectedSeed)) // 씨앗을 사용하면 true 반환
            {
                farmSlots[slotIndex].PlantSeed(selectedSeed);
                farmSlots[slotIndex].PlantBtn.image.color 
                    = new Color(farmSlots[slotIndex].PlantBtn.image.color.r,
                    farmSlots[slotIndex].PlantBtn.image.color.g, 
                    farmSlots[slotIndex].PlantBtn.image.color.b,0);
            }
        }
        else if (selectedSeed == null && !isWateringMode && farmSlots[slotIndex].isGrown)
        {
            // 작물 수확 조건
            HarvestCrops(slotIndex);
        }
    }
    public void HarvestCrops(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < farmSlots.Length)
        {
            ItemObject harvestedCrops = farmSlots[slotIndex].HarvestCrops();

            if (harvestedCrops != null)
            {
                inventory.items.Add(harvestedCrops); // 인벤토리에 추가
                farmSlots[slotIndex].ResetSlot(); // 슬롯 초기화
            }
        }
    }
}
