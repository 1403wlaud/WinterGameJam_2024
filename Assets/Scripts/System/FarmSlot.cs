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
    public GameObject text;
    private Sprite defaultSprite; // �⺻ ��������Ʈ ����
    private int plantedDay; // ������ �ɾ��� ��

    private void OnEnable()
    {
        PlantBtn = GetComponentInChildren<Button>();
        CropsSprite = GetComponentInChildren<Image>();
        timeManager = GameObject.Find("GameManager").GetComponent<TimeManager>();
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
            plantedDay = timeManager.Day; // ������ �ɾ��� �� ����
        }
    }

    public void WaterPlant()
    {
        if (currentSeed != null && !isGrown)
        {
            waterScore++;
        }
    }

    private void Update()
    {
        if (currentSeed == null || isGrown)
            return;

        // �۹��� �ڶ� ���� Ȯ��
        int daysSincePlanted = timeManager.Day - plantedDay; // �ɾ��� ���� ���� �� ���

        if (daysSincePlanted >= currentSeed.Seed_TimeTaken && waterScore >= 10)
        {
            foreach (var item in items)
            {
                if (currentSeed.Item_Crops == item.Item_Crops &&
                    item.Item_Type == ItemType.crops)
                {
                    text.gameObject.SetActive(true);
                    currentCropsItem = item; // �۹��� ����
                    isGrown = true;
                    break;
                }
            }
        }
    }

    public ItemObject HarvestCrops()
    {
        if (isGrown && currentSeed != null)
        {
            ItemObject harvestedItem = currentCropsItem;
            ResetSlot(); // ���� �ʱ�ȭ
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
        plantedDay = 0; // �ʱ�ȭ
        PlantBtn.image.sprite = defaultSprite; // ��������Ʈ �ʱ�ȭ
        text.gameObject.SetActive (false);
    }
}
