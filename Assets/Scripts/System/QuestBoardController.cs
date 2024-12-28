using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestBoardController : MonoBehaviour
{
    public SCTOBJQuest AssignedQuest; // �� ���忡 �Ҵ�� ����Ʈ
    public Text ChaName;
    public Image ChaImage;
    public Text OderTxt;
    public Text RewardTxt;
    public Button SubmitButton;
    public Button RewardButton;
    public Image ClearImage;

    [HideInInspector]
    public int QuestClearDay;

    private ItemInventory playerInventory;
    private void Start()
    {
        RewardButton.gameObject.SetActive(false); // �ʱ� ��Ȱ��ȭ
    }

    public void Initialize(SCTOBJQuest quest, ItemInventory inventory)
    {
        AssignedQuest = quest; // �� ���忡 �Ҵ�� ����Ʈ ����
        playerInventory = inventory;

        // UI �ʱ�ȭ
        UpdateUI();

        // ��ư �̺�Ʈ ����
        SubmitButton.onClick.RemoveAllListeners();
        SubmitButton.onClick.AddListener(SubmitQuest);

        RewardButton.onClick.RemoveAllListeners();
        RewardButton.onClick.AddListener(ReceiveReward);
        
    }

    /// ����Ʈ ������ UI ������Ʈ
    public void UpdateUI()
    {
        if (AssignedQuest == null || playerInventory == null) return;

        int playerItemCount1 = GetItemCountInInventory(AssignedQuest.Q_NeedObj);
        int playerItemCount2 = AssignedQuest.Q_NeedObj2 != null ? GetItemCountInInventory(AssignedQuest.Q_NeedObj2) : 0;

        // �غ� �ؽ�Ʈ ������Ʈ
        OderTxt.text = $"{AssignedQuest.Q_NeedObj.Item_Example} {AssignedQuest.Q_NeedItemCount} / {playerItemCount1}\n" +
                       (AssignedQuest.Q_NeedObj2 != null
                           ? $"{AssignedQuest.Q_NeedObj2.Item_Example} {AssignedQuest.Q_NeedItemCount2} / {playerItemCount2}"
                           : "");
        ChaImage.sprite = AssignedQuest.Q_CharacterFaceImg;

        // ���� �ؽ�Ʈ ������Ʈ
        RewardTxt.text = AssignedQuest.Q_RewardMoney > 0
            ? $"{AssignedQuest.Q_RewardMoney} G"
            : (AssignedQuest.Q_RewardItem != null
                ? $"{AssignedQuest.Q_RewardItem.Item_Name} x {AssignedQuest.Q_ItemCount}"
                : "������ �����ϴ�");

        // ���� ��ư ���� ����
        UpdateSubmitButtonState(playerItemCount1, playerItemCount2);

        
    }

    private int GetItemCountInInventory(ItemObject item)
    {
        int count = 0;
        foreach (var playerItem in playerInventory.items)
        {
            if (playerItem == item)
            {
                count++;
            }
        }
        return count;
    }

    private void UpdateSubmitButtonState(int count1, int count2)
    {
        bool canSubmit = count1 >= AssignedQuest.Q_NeedItemCount &&
                         (AssignedQuest.Q_NeedObj2 == null || count2 >= AssignedQuest.Q_NeedItemCount2);

        SubmitButton.interactable = canSubmit;
        SubmitButton.GetComponent<Image>().color = canSubmit ? Color.white : Color.gray;
    }

    private void SubmitQuest()
    {
        if (AssignedQuest == null || playerInventory == null) return;

        int playerItemCount1 = GetItemCountInInventory(AssignedQuest.Q_NeedObj);
        int playerItemCount2 = AssignedQuest.Q_NeedObj2 != null ? GetItemCountInInventory(AssignedQuest.Q_NeedObj2) : 0;

        if (playerItemCount1 >= AssignedQuest.Q_NeedItemCount &&
            (AssignedQuest.Q_NeedObj2 == null || playerItemCount2 >= AssignedQuest.Q_NeedItemCount2))
        {
            // �ʿ��� ������ ����
            for (int i = 0; i < AssignedQuest.Q_NeedItemCount; i++)
                playerInventory.items.Remove(AssignedQuest.Q_NeedObj);

            if (AssignedQuest.Q_NeedObj2 != null)
            {
                for (int i = 0; i < AssignedQuest.Q_NeedItemCount2; i++)
                    playerInventory.items.Remove(AssignedQuest.Q_NeedObj2);
            }

            // ���� ��ư ��Ȱ��ȭ
            SubmitButton.interactable = false;
            SubmitButton.GetComponent<Image>().color = Color.gray;

            // ���� ��ư Ȱ��ȭ
            RewardButton.gameObject.SetActive(true);
            RewardButton.interactable = true;
            RewardButton.GetComponent<Image>().color = Color.white;

            UpdateUI();

            // ���� ����ȭ
            playerInventory.SyncSlotsWithInventory();

            QuestClearDay = AssignedQuest.Q_Day;
        }
    }

    private void ReceiveReward()
    {
        if (AssignedQuest == null || playerInventory == null) return;

        // ���� ����
        if (AssignedQuest.Q_RewardMoney > 0)
        {
            playerInventory.Money += AssignedQuest.Q_RewardMoney;
        }
        else if (AssignedQuest.Q_RewardItem != null)
        {
            for (int i = 0; i < AssignedQuest.Q_ItemCount; i++)
                playerInventory.items.Add(AssignedQuest.Q_RewardItem);
        }

        // ���� ��ư ��Ȱ��ȭ
        RewardButton.interactable = false;
        RewardButton.GetComponent<Image>().color = Color.gray;

        ClearImage.gameObject.SetActive(true);
        // UI ������Ʈ
        UpdateUI();

        // ���� ����ȭ
        playerInventory.SyncSlotsWithInventory();
    }
}
