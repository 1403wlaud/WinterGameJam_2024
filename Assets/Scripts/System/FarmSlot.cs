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
    private Sprite defaultSprite; // 기본 스프라이트 저장

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
            waterScore = 0; // 물 초기화
            isGrown = false;
            Debug.Log($"씨앗 {seed.Item_Name}이 심어졌습니다.");
        }
    }

    public void WaterPlant()
    {
        if (currentSeed != null && !isGrown)
        {
            waterScore++;
            Debug.Log("물을 주었습니다. 현재 물 점수: " + waterScore);
        }
    }

    private void Update()
    {

        if (currentSeed == null || isGrown)
            return;
        Debug.Log(currentSeed);
        // 성장 확인
        if (timeManager.Day >= currentSeed.Seed_TimeTaken && waterScore >= 10)
        {
            foreach (var item in items)
            {
                if (currentSeed.Item_Crops == item.Item_Crops &&
                    item.Item_Type == ItemType.crops)
                {
                    currentCropsItem = item; // 작물로 성장
                    isGrown = true;
                    Debug.Log($"{currentCropsItem.Item_Name}이 성장했습니다!");
                    break;
                }
            }
        }
    }

    public ItemObject HarvestCrops()
    {
        if (isGrown && currentSeed != null)
        {
            Debug.Log($"{currentSeed.Item_Name}이 수확되었습니다!");
            ItemObject harvestedItem = currentSeed;
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
        PlantBtn.image.sprite = defaultSprite; // 스프라이트 초기화
        Debug.Log("슬롯이 초기화되었습니다.");
    }

}
