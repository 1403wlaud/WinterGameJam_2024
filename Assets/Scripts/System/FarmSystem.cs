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
    private Outline selectedOutline; // 현재 선택된 버튼의 Outline 참조

    private void Start()
    {
        SetupSeedButtons();
        SetupSlotButtons();

        // 물 양동이 버튼 이벤트 설정
        waterButton.onClick.AddListener(ToggleWateringMode);
        AddOutlineComponent(waterButton); // 물 양동이 버튼에 Outline 추가
    }

    /// <summary>
    /// 씨앗 버튼에 클릭 이벤트 추가
    /// </summary>
    private void SetupSeedButtons()
    {
        for (int i = 0; i < seedButtons.Length; i++)
        {
            int index = i;
            AddOutlineComponent(seedButtons[i]); // Outline 컴포넌트 추가
            seedButtons[i].onClick.AddListener(() => ToggleSeedSelection(seedItems[index], seedButtons[index]));
        }
    }

    /// <summary>
    /// 슬롯 버튼에 클릭 이벤트 추가
    /// </summary>
    private void SetupSlotButtons()
    {
        for (int i = 0; i < farmSlots.Length; i++)
        {
            int slotIndex = i;
            farmSlots[i].PlantBtn.onClick.AddListener(() => OnSlotButtonClicked(slotIndex));
        }
    }

    /// <summary>
    /// 물 양동이 모드 토글
    /// </summary>
    private void ToggleWateringMode()
    {
        if (isWateringMode)
        {
            isWateringMode = false; // 물 주기 모드 비활성화
            SetOutline(waterButton, false);
            Debug.Log("물 주기 모드가 종료되었습니다.");
        }
        else
        {
            selectedSeed = null; // 씨앗 선택 해제
            isWateringMode = true; // 물 주기 모드 활성화
            SetOutline(waterButton, true);
            ClearAllOtherOutlines(waterButton);
            Debug.Log("물 주기 모드가 활성화되었습니다.");
        }
    }

    /// <summary>
    /// 씨앗 선택 토글
    /// </summary>
    private void ToggleSeedSelection(ItemObject seed, Button button)
    {
        if (selectedSeed == seed)
        {
            selectedSeed = null; // 같은 씨앗 버튼 다시 누르면 해제
            SetOutline(button, false);
            Debug.Log("씨앗 선택이 해제되었습니다.");
        }
        else
        {
            selectedSeed = seed; // 새로운 씨앗 선택
            isWateringMode = false; // 물 주기 모드 비활성화
            SetOutline(button, true);
            ClearAllOtherOutlines(button);
            Debug.Log($"씨앗 {seed.Item_Name}이 선택되었습니다.");
        }
    }

    /// <summary>
    /// 슬롯 버튼 클릭 시 동작
    /// </summary>
    private void OnSlotButtonClicked(int slotIndex)
    {
        if (isWateringMode)
        {
            // 물 주기 모드
            farmSlots[slotIndex].WaterPlant();
            Debug.Log($"슬롯 {slotIndex}에 물을 주었습니다. 현재 물 점수: {farmSlots[slotIndex].waterScore}");
        }
        else if (selectedSeed != null && farmSlots[slotIndex].currentSeed == null)
        {
            // 씨앗 심기
            farmSlots[slotIndex].PlantSeed(selectedSeed);
            farmSlots[slotIndex].PlantBtn.image.sprite = null; // 스프라이트 숨김
            Debug.Log($"슬롯 {slotIndex}에 씨앗 {selectedSeed.Item_Name}이 심어졌습니다.");
        }
        else if (selectedSeed == null && !isWateringMode && farmSlots[slotIndex].isGrown)
        {
            // 작물 수확 조건
            HarvestCrops(slotIndex);
        }
    }

    /// <summary>
    /// 작물 수확
    /// </summary>
    public void HarvestCrops(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < farmSlots.Length)
        {
            ItemObject harvestedCrops = farmSlots[slotIndex].HarvestCrops();

            if (harvestedCrops != null)
            {
                inventory.items.Add(harvestedCrops); // 인벤토리에 추가
                farmSlots[slotIndex].ResetSlot(); // 슬롯 초기화
                Debug.Log($"{harvestedCrops.Item_Name}을(를) 수확하여 인벤토리에 추가했습니다.");
            }
            else
            {
                Debug.Log("수확할 작물이 없습니다.");
            }
        }
    }

    /// <summary>
    /// Outline 컴포넌트 추가
    /// </summary>
    private void AddOutlineComponent(Button button)
    {
        if (button.GetComponent<Outline>() == null)
        {
            Outline outline = button.gameObject.AddComponent<Outline>();
            outline.enabled = false; // 초기에는 비활성화
            outline.effectColor = Color.yellow; // 선택 시 강조 색상
            outline.effectDistance = new Vector2(5, 5); // 강조 효과 두께
        }
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
}
