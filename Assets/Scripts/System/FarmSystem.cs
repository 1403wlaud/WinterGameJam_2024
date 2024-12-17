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
    private Outline selectedOutline; // ���� ���õ� ��ư�� Outline ����

    private void Start()
    {
        SetupSeedButtons();
        SetupSlotButtons();

        // �� �絿�� ��ư �̺�Ʈ ����
        waterButton.onClick.AddListener(ToggleWateringMode);
        AddOutlineComponent(waterButton); // �� �絿�� ��ư�� Outline �߰�
    }

    /// <summary>
    /// ���� ��ư�� Ŭ�� �̺�Ʈ �߰�
    /// </summary>
    private void SetupSeedButtons()
    {
        for (int i = 0; i < seedButtons.Length; i++)
        {
            int index = i;
            AddOutlineComponent(seedButtons[i]); // Outline ������Ʈ �߰�
            seedButtons[i].onClick.AddListener(() => ToggleSeedSelection(seedItems[index], seedButtons[index]));
        }
    }

    /// <summary>
    /// ���� ��ư�� Ŭ�� �̺�Ʈ �߰�
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
    /// �� �絿�� ��� ���
    /// </summary>
    private void ToggleWateringMode()
    {
        if (isWateringMode)
        {
            isWateringMode = false; // �� �ֱ� ��� ��Ȱ��ȭ
            SetOutline(waterButton, false);
            Debug.Log("�� �ֱ� ��尡 ����Ǿ����ϴ�.");
        }
        else
        {
            selectedSeed = null; // ���� ���� ����
            isWateringMode = true; // �� �ֱ� ��� Ȱ��ȭ
            SetOutline(waterButton, true);
            ClearAllOtherOutlines(waterButton);
            Debug.Log("�� �ֱ� ��尡 Ȱ��ȭ�Ǿ����ϴ�.");
        }
    }

    /// <summary>
    /// ���� ���� ���
    /// </summary>
    private void ToggleSeedSelection(ItemObject seed, Button button)
    {
        if (selectedSeed == seed)
        {
            selectedSeed = null; // ���� ���� ��ư �ٽ� ������ ����
            SetOutline(button, false);
            Debug.Log("���� ������ �����Ǿ����ϴ�.");
        }
        else
        {
            selectedSeed = seed; // ���ο� ���� ����
            isWateringMode = false; // �� �ֱ� ��� ��Ȱ��ȭ
            SetOutline(button, true);
            ClearAllOtherOutlines(button);
            Debug.Log($"���� {seed.Item_Name}�� ���õǾ����ϴ�.");
        }
    }

    /// <summary>
    /// ���� ��ư Ŭ�� �� ����
    /// </summary>
    private void OnSlotButtonClicked(int slotIndex)
    {
        if (isWateringMode)
        {
            // �� �ֱ� ���
            farmSlots[slotIndex].WaterPlant();
            Debug.Log($"���� {slotIndex}�� ���� �־����ϴ�. ���� �� ����: {farmSlots[slotIndex].waterScore}");
        }
        else if (selectedSeed != null && farmSlots[slotIndex].currentSeed == null)
        {
            // ���� �ɱ�
            farmSlots[slotIndex].PlantSeed(selectedSeed);
            farmSlots[slotIndex].PlantBtn.image.sprite = null; // ��������Ʈ ����
            Debug.Log($"���� {slotIndex}�� ���� {selectedSeed.Item_Name}�� �ɾ������ϴ�.");
        }
        else if (selectedSeed == null && !isWateringMode && farmSlots[slotIndex].isGrown)
        {
            // �۹� ��Ȯ ����
            HarvestCrops(slotIndex);
        }
    }

    /// <summary>
    /// �۹� ��Ȯ
    /// </summary>
    public void HarvestCrops(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < farmSlots.Length)
        {
            ItemObject harvestedCrops = farmSlots[slotIndex].HarvestCrops();

            if (harvestedCrops != null)
            {
                inventory.items.Add(harvestedCrops); // �κ��丮�� �߰�
                farmSlots[slotIndex].ResetSlot(); // ���� �ʱ�ȭ
                Debug.Log($"{harvestedCrops.Item_Name}��(��) ��Ȯ�Ͽ� �κ��丮�� �߰��߽��ϴ�.");
            }
            else
            {
                Debug.Log("��Ȯ�� �۹��� �����ϴ�.");
            }
        }
    }

    /// <summary>
    /// Outline ������Ʈ �߰�
    /// </summary>
    private void AddOutlineComponent(Button button)
    {
        if (button.GetComponent<Outline>() == null)
        {
            Outline outline = button.gameObject.AddComponent<Outline>();
            outline.enabled = false; // �ʱ⿡�� ��Ȱ��ȭ
            outline.effectColor = Color.yellow; // ���� �� ���� ����
            outline.effectDistance = new Vector2(5, 5); // ���� ȿ�� �β�
        }
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
}
