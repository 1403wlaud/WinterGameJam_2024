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
    private Image ItemImage;
    private Sprite defaultSprite; // 기본 스프라이트 저장
    private int plantedDay; // 씨앗이 심어진 날

    private void OnEnable()
    {
        PlantBtn = GetComponentInChildren<Button>();
        CropsSprite = GetComponentInChildren<Image>();
        ItemImage = transform.GetChild(0).GetComponent<Image>();
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
            ItemImage.gameObject.SetActive(true);
            ItemImage.sprite = currentSeed.Seed_GrrowSprites[0]; // 첫 번째 스프라이트
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

        // 심어진 이후 경과한 일수 계산
        int daysSincePlanted = timeManager.Day - plantedDay;

        // 경과 일수에 따라 성장 단계 스프라이트 변경
        UpdatePlantSprite(daysSincePlanted);

        // 최종적으로 작물이 완전히 자랐는지 확인
        if (daysSincePlanted >= currentSeed.Seed_TimeTaken && waterScore >= 3)
        {
            foreach (var item in items)
            {
                if (currentSeed.Item_Crops == item.Item_Crops &&
                    item.Item_Type == ItemType.crops)
                {
                    text.gameObject.SetActive(true);
                    currentCropsItem = item; // 작물로 성장
                    ItemImage.sprite = currentSeed.Seed_GrrowSprites[currentSeed.Seed_GrrowSprites.Length - 1]; // 최종 스프라이트
                    isGrown = true;
                    break;
                }
            }
        }
    }

    private void UpdatePlantSprite(int daysSincePlanted)
    {
        if (currentSeed == null || currentSeed.Seed_GrrowSprites.Length == 0)
            return;

        // 현재 성장 단계 결정
        int cumulativeDays = 0;
        for (int i = 0; i < currentSeed.Seed_GrowthDurations.Length; i++)
        {
            cumulativeDays += currentSeed.Seed_GrowthDurations[i];
            if (daysSincePlanted < cumulativeDays)
            {
                ItemImage.sprite = currentSeed.Seed_GrrowSprites[i];
                return;
            }
        }

        // 모든 단계를 넘어갔을 경우 마지막 스프라이트 유지
        ItemImage.sprite = currentSeed.Seed_GrrowSprites[currentSeed.Seed_GrrowSprites.Length - 1];
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
        ItemImage.gameObject.SetActive(false);
        PlantBtn.image.color = new Color(
            PlantBtn.image.color.r, PlantBtn.image.color.g,
            PlantBtn.image.color.b, 255);
        PlantBtn.image.sprite = defaultSprite; // 스프라이트 초기화
        text.gameObject.SetActive(false);
    }
}
