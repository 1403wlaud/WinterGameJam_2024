using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoticeBoardManager : MonoBehaviour
{
    [SerializeField] private List<Quest> quests; // ����Ʈ ����Ʈ
    public Image ChImg;
    public Text ChNmae;
    public Text OderTxt;
    public Text RewardTxt;

    public Image Panel; // �г� UI
    public TimeManager timeManager; // ��¥ ����

    private Quest activeQuest = null; // ���� Ȱ��ȭ�� ����Ʈ
    private int lastCheckedDay = 0; // ���������� Ȯ���� ��¥

    private void Update()
    {
        CheckForNewQuest();
        UpdatePanel();
    }

    // ��¥ ���� �� ���ο� ����Ʈ Ȯ��
    private void CheckForNewQuest()
    {
        // ��¥�� ����Ǿ����� Ȯ��
        if (timeManager.Day != lastCheckedDay)
        {
            lastCheckedDay = timeManager.Day; // ���� ��¥ ����

            // ���ο� ����Ʈ�� �ִ��� Ȯ��
            Quest newQuest = GetQuestForToday();
            if (newQuest != null)
            {
                activeQuest = newQuest; // ���ο� ����Ʈ�� ����
            }
        }
    }

    // ���� �Ǵ� ���� �ֱٿ� Ȱ��ȭ�� ����Ʈ�� ��ȯ
    private Quest GetQuestForToday()
    {
        Quest latestQuest = null;

        foreach (Quest quest in quests)
        {
            // ���� ��¥ ������ ù ��° ����Ʈ�� ã��
            if (timeManager.Day >= quest.Q_Day)
            {
                latestQuest = quest; // ���� �ֱ� ��¥�� ����Ʈ ����
            }

            // ��¥�� ���� ��¥���� �ڶ�� ���� ����
            if (timeManager.Day < quest.Q_Day)
            {
                break;
            }
        }

        return latestQuest;
    }

    // �г� UI ������Ʈ
    private void UpdatePanel()
    {
        if (activeQuest != null)
        {
            Panel.gameObject.SetActive(true); // �г� Ȱ��ȭ
            UpdateUI(activeQuest);
        }
        else if(activeQuest==null || activeQuest.Q_CheckQuest)
        {
            Panel.gameObject.SetActive(false); // �г� ��Ȱ��ȭ
        }
    }

    // UI�� ����Ʈ ���� ������Ʈ
    private void UpdateUI(Quest quest)
    {
        ChImg.sprite = quest.Q_CharacterFaceImg;
        ChNmae.text = quest.Q_CharacterName;
        OderTxt.text = $"{quest.Q_NeedObj.Item_Name} {quest.Q_NeedItemCount}��";
        RewardTxt.text = quest.Q_RewardMoney.ToString();
    }
}
