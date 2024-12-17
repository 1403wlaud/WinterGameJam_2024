using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FarmSlot : MonoBehaviour
{
    public ItemObject currentSeed; // �ɾ��� ����
    public ItemObject currentCropsItem; // ������ �۹�
    public List<ItemObject> items; // �۹� ������ ����Ʈ (ScriptableObject)
    public int waterScore; // ���� �� (������ �ý���)
    public bool isGrown; // �۹��� �����ߴ��� ����
    private TimeManager timeManager;
    public Button PlantBtn;
    public Image CropsSprite;
    private Sprite defaultSprite; // �⺻ ��������Ʈ ����

    private void OnEnable()
    {
        PlantBtn=GetComponentInChildren<Button>();
        CropsSprite=GetComponentInChildren<Image>();
        timeManager = GameObject.Find("GameManager").
            GetComponent<TimeManager>();
        defaultSprite = PlantBtn.image.sprite;
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
        Debug.Log(currentSeed);
        // ���� Ȯ��
        if (timeManager.Day >= currentSeed.Seed_TimeTaken && waterScore >= 10)
        {
            foreach (var item in items)
            {
                if (currentSeed.Item_Crops == item.Item_Crops &&
                    item.Item_Type == ItemType.crops)
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
        if (isGrown && currentSeed != null)
        {
            Debug.Log($"{currentSeed.Item_Name}�� ��Ȯ�Ǿ����ϴ�!");
            ItemObject harvestedItem = currentSeed;
            return harvestedItem; // ��Ȯ�� �۹� ��ȯ
        }
        return null;
    }

    public void ResetSlot()
    {
        currentSeed = null;
        currentCropsItem = null;
        waterScore = 0;
        isGrown = false;
        PlantBtn.image.sprite = defaultSprite; // ��������Ʈ �ʱ�ȭ
        Debug.Log("������ �ʱ�ȭ�Ǿ����ϴ�.");
    }

}
