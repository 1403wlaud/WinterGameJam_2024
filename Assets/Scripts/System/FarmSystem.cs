using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FarmSystem : MonoBehaviour
{
    [Header("���� ���� ����")]
    public FarmSlot[] farmSlots; // ���� ���� �迭
    [Header("�÷��̾� �κ��丮")]
    public ItemInventory inventory; // �÷��̾� �κ��丮
    [Header("���õ� ����")]
    public ItemObject selectedSeed; // ���� ���õ� ����
    [Header("�� �絿�� ��ư")]
    public Button waterButton; // �� �絿�� ��ư
    private bool isWateringMode = false; // �� �ֱ� ��� ����

    [Header("���� ��ư ����")]
    public Button[] seedButtons; // ���� ��ư �迭
    public ItemObject[] seedItems; // �� ���� ��ư�� �����ϴ� ���� ������
    public Text[] seedCountTexts; // ���� ���� ǥ���� �ؽ�Ʈ �迭

    private Button selectedButton; // ���õ� ��ư ����

    private void Start()
    {
        SetupSeedButtons();
        SetupSlotButtons();

        // �� �絿�� ��ư �̺�Ʈ ����
        waterButton.onClick.RemoveAllListeners();
        waterButton.onClick.AddListener(ToggleWateringMode);
        AddOutlineComponent(waterButton);
    }
    private void Update()
    {
        UpdateSeedCounts(); // ���� ���� �ǽð����� ������Ʈ
    }

    private void SetupSeedButtons()
    {
        for (int i = 0; i < seedButtons.Length; i++)
        {
            int index = i; // ���� ������ ĸó
            AddOutlineComponent(seedButtons[index]);

            // ���� �̺�Ʈ ����
            seedButtons[index].onClick.RemoveAllListeners();

            // Ŭ�� �̺�Ʈ �߰�
            seedButtons[index].onClick.AddListener(() => OnSeedButtonClicked(index));
        }
    }

    private void UpdateSeedCounts()
    {
        for (int i = 0; i < seedItems.Length; i++)
        {
            int count = GetSeedCount(seedItems[i]); // �κ��丮���� ���� �� Ȯ��
            seedCountTexts[i].text = count > 0 ? count.ToString() : "0"; // Text�� ���� ǥ��
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
    /// �� �絿�� ��� ���
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
    /// ���� ���� ���
    /// </summary>
    private void OnSeedButtonClicked(int index)
    {
        Button clickedButton = seedButtons[index];

        if (selectedButton == clickedButton) // �̹� ���õ� ��ư Ŭ�� �� ����
        {
            selectedSeed = null;
            SetOutline(clickedButton, false);
            selectedButton = null;
        }
        else // ���ο� ��ư ����
        {
            selectedSeed = seedItems[index];
            isWateringMode = false;
            SetOutline(clickedButton, true);
            ClearAllOtherOutlines(clickedButton);
            selectedButton = clickedButton; // ���� ���õ� ��ư ������Ʈ
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
            outline.enabled = false; // �ʱ⿡�� ��Ȱ��ȭ
            outline.effectColor = Color.yellow; // ���� ����
            outline.effectDistance = new Vector2(5, 5); // ���� ȿ�� �Ÿ�
        }
    }

    /// <summary>
    /// ������ �κ��丮���� �ϳ� �Ҹ�
    /// </summary>
    private bool ConsumeSeed(ItemObject seed)
    {
        for (int i = 0; i < inventory.items.Count; i++)
        {
            if (inventory.items[i].Item_Name == seed.Item_Name) // �̸����� ��
            {
                inventory.items.RemoveAt(i); // ������ ����
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Ư�� ��ư�� Outline ����
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
    /// �ٸ� ��ư���� Outline ����
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
            if (ConsumeSeed(selectedSeed)) // ������ ����ϸ� true ��ȯ
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
            // �۹� ��Ȯ ����
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
                inventory.items.Add(harvestedCrops); // �κ��丮�� �߰�
                farmSlots[slotIndex].ResetSlot(); // ���� �ʱ�ȭ
            }
        }
    }
}
