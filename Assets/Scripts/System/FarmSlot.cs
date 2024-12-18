using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FarmSlot : MonoBehaviour
{
    public ItemObject currentSeed; // 심어진 씨앗
    public ItemObject currentCropsItem; // 성장한 작물
    public List<ItemObject> items; // 작물 아이템 리스트 (ScriptableObject)
    public int waterScore; // 물의 양 (간단한 시스템)
    public bool isGrown; // 작물이 성장했는지 여부
    private TimeManager timeManager;
    public Button PlantBtn;
    public Image CropsSprite;
    public GameObject text;
    private Sprite defaultSprite; // 기본 스프라이트 저장
    private int plantedDay; // 씨앗이 심어진 날

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
            waterScore = 0; // 물 초기화
            isGrown = false;
            plantedDay = timeManager.Day; // 씨앗이 심어진 날 저장
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

        // 작물이 자랄 조건 확인
        int daysSincePlanted = timeManager.Day - plantedDay; // 심어진 이후 지난 날 계산

        if (daysSincePlanted >= currentSeed.Seed_TimeTaken && waterScore >= 10)
        {
            foreach (var item in items)
            {
                if (currentSeed.Item_Crops == item.Item_Crops &&
                    item.Item_Type == ItemType.crops)
                {
                    text.gameObject.SetActive(true);
                    currentCropsItem = item; // 작물로 성장
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
            ResetSlot(); // 슬롯 초기화
            return harvestedItem; // 수확된 작물 반환
        }
        return null;
    }

    public void ResetSlot()
    {
        currentSeed = null;
        currentCropsItem = null;
        waterScore = 0;
        isGrown = false;
        plantedDay = 0; // 초기화
        PlantBtn.image.sprite = defaultSprite; // 스프라이트 초기화
        text.gameObject.SetActive (false);
    }
}
