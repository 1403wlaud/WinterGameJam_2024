using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClick : MonoBehaviour
{
    public Button[] Buttons; // 버튼 배열
    public FarmSystem FarmSystem; // FarmSystem 참조
    public ItemObject[] seedItems; // 버튼마다 대응하는 씨앗 아이템 배열

    private void Start()
    {
        // 각 버튼에 클릭 이벤트 추가
        for (int i = 0; i < Buttons.Length; i++)
        {
            int index = i; // 로컬 변수로 캡처
            Buttons[i].onClick.AddListener(() => OnButtonClicked(index));
        }
    }

    /// <summary>
    /// 버튼 클릭 시 FarmSystem에 selectedSeed를 전달
    /// </summary>
    /// <param name="index">클릭된 버튼의 인덱스</param>
    private void OnButtonClicked(int index)
    {
        if (FarmSystem != null && seedItems != null && index < seedItems.Length)
        {
            FarmSystem.selectedSeed = seedItems[index]; // 선택된 씨앗 전달
            Debug.Log($"씨앗 {seedItems[index].Item_Name}이 선택되었습니다.");
        }
        else
        {
            Debug.LogWarning("FarmSystem이나 seedItems 설정이 잘못되었습니다.");
        }
    }
}
