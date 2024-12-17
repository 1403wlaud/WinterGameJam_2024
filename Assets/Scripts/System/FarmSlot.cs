using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmSlot : MonoBehaviour
{
    public ItemObject currentSeed; // �ɾ��� ����
    public ItemObject currentCropsItem; // ������ �۹�
    public List<ItemObject> items; // �۹� ������ ����Ʈ (ScriptableObject)
    public int waterScore; // ���� �� (������ �ý���)
    public bool isGrown; // �۹��� �����ߴ��� ����
    private TimeManager timeManager;

    private void Start()
    {
        timeManager = GameObject.Find("GameManager").
            GetComponent<TimeManager>();
    }

    public void PlantSeed(ItemObject seed)
    {
        if (seed == null) return;
        if (currentSeed == null)
        {
            currentSeed = seed;
            waterScore = 0; // �� �ʱ�ȭ
            isGrown = false;
            Debug.Log($"���� {seed.Item_Name}�� �ɾ������ϴ�.");
        }
    }

    public void WaterPlant()
    {
        if (currentSeed != null && !isGrown)
        {
            waterScore++;
            Debug.Log("���� �־����ϴ�. ���� �� ����: " + waterScore);
        }
    }

    private void Update()
    {
        if (currentSeed == null || isGrown)
            return;

        // ���� Ȯ��
        if (timeManager.Day >= currentSeed.Seed_TimeTaken && waterScore >= 10)
        {
            foreach (var item in items)
            {
                if (currentSeed.Item_Crops == item.Item_Crops)
                {
                    currentCropsItem = item; // �۹��� ����
                    isGrown = true;
                    Debug.Log($"{currentCropsItem.Item_Name}�� �����߽��ϴ�!");
                    break;
                }
            }
        }
    }

    public ItemObject HarvestCrops()
    {
        if (isGrown && currentCropsItem != null)
        {
            ItemObject harvestedCrops = currentCropsItem;
            ResetSlot();
            return harvestedCrops;
        }
        return null;
    }

    private void ResetSlot()
    {
        currentSeed = null;
        currentCropsItem = null;
        isGrown = false;
        waterScore = 0;
    }

}
