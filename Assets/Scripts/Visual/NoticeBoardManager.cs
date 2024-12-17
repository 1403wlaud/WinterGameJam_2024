using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoticeBoardManager : MonoBehaviour
{
    [SerializeField] private List<Quest> quests; // 퀘스트 리스트
    public Image ChImg;
    public Text ChNmae;
    public Text OderTxt;
    public Text RewardTxt;

    public Image Panel; // 패널 UI
    public TimeManager timeManager; // 날짜 관리

    private Quest activeQuest = null; // 현재 활성화된 퀘스트
    private int lastCheckedDay = 0; // 마지막으로 확인한 날짜

    private void Update()
    {
        CheckForNewQuest();
        UpdatePanel();
    }

    // 날짜 변경 및 새로운 퀘스트 확인
    private void CheckForNewQuest()
    {
        // 날짜가 변경되었는지 확인
        if (timeManager.Day != lastCheckedDay)
        {
            lastCheckedDay = timeManager.Day; // 현재 날짜 갱신

            // 새로운 퀘스트가 있는지 확인
            Quest newQuest = GetQuestForToday();
            if (newQuest != null)
            {
                activeQuest = newQuest; // 새로운 퀘스트로 갱신
            }
        }
    }

    // 오늘 또는 가장 최근에 활성화된 퀘스트를 반환
    private Quest GetQuestForToday()
    {
        Quest latestQuest = null;

        foreach (Quest quest in quests)
        {
            // 오늘 날짜 이후의 첫 번째 퀘스트를 찾음
            if (timeManager.Day >= quest.Q_Day)
            {
                latestQuest = quest; // 가장 최근 날짜의 퀘스트 갱신
            }

            // 날짜가 현재 날짜보다 뒤라면 루프 중지
            if (timeManager.Day < quest.Q_Day)
            {
                break;
            }
        }

        return latestQuest;
    }

    // 패널 UI 업데이트
    private void UpdatePanel()
    {
        if (activeQuest != null)
        {
            Panel.gameObject.SetActive(true); // 패널 활성화
            UpdateUI(activeQuest);
        }
        else if(activeQuest==null || activeQuest.Q_CheckQuest)
        {
            Panel.gameObject.SetActive(false); // 패널 비활성화
        }
    }

    // UI에 퀘스트 내용 업데이트
    private void UpdateUI(Quest quest)
    {
        ChImg.sprite = quest.Q_CharacterFaceImg;
        ChNmae.text = quest.Q_CharacterName;
        OderTxt.text = $"{quest.Q_NeedObj.Item_Name} {quest.Q_NeedItemCount}개";
        RewardTxt.text = quest.Q_RewardMoney.ToString();
    }
}
