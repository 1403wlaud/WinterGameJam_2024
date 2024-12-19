using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoticeBoardManager : MonoBehaviour
{
    public GameObject QuestBordPrefab;
    public Transform InstancePos;
    public TimeManager timeManager;
    public ItemInventory playerInventory;
    public GameObject text;

    private List<GameObject> activeQuestBoards = new List<GameObject>();
    [SerializeField] private List<SCTOBJQuest> quests;

    private void Update()
    {
        if (GameObject.Find("Player(Clone)") != null)
            playerInventory = GameObject.Find("Player(Clone)").GetComponent<ItemInventory>();
        CheckForNewQuests(); //퀘스트 확인

        //UI업데이트
        if(activeQuestBoards.Count > 0)
            for(int i = 0; i < activeQuestBoards.Count; i++)
                for(int j = 0;j<quests.Count; j++)
                    if(i==j) UpdateQuestBoardUI(activeQuestBoards[i], quests[j]);

        if (activeQuestBoards.Count==0)
            text.gameObject.SetActive(true);
        else
            text.gameObject.SetActive(false);   
    }

    private void CheckForNewQuests()
    {
        // 퀘스트 보드를 생성하는 로직 호출
        SpawnQuestBoards();
    }

    private void SpawnQuestBoards()
    {
        foreach (SCTOBJQuest quest in quests)
        {
            if (IsQuestAlreadyActive(quest))
            {

                Debug.Log($"이미 활성화된 퀘스트: {quest.Q_CharacterName}");
                continue;
            }

            if (quest.Q_Day == timeManager.Day)
            {
                Debug.Log($"새 퀘스트 생성: {quest.Q_CharacterName}");
                GameObject questBoard = Instantiate(QuestBordPrefab, InstancePos);

                if (questBoard == null)
                {
                    Debug.LogError("QuestBoardPrefab 인스턴스화 실패!");
                    continue;
                }

                // UI 초기화는 UpdateQuestBoardUI로 처리
                
                activeQuestBoards.Add(questBoard);
            }
        }
    }

    private bool IsQuestAlreadyActive(SCTOBJQuest quest)
    {
        foreach (GameObject questBoard in activeQuestBoards)
        {
            QuestBoardController controller = questBoard.GetComponent<QuestBoardController>();
            if (controller != null && controller.AssignedQuest == quest)
                return true;
        }
        return false;
    }

    private void UpdateQuestBoardUI(GameObject questBoard, SCTOBJQuest quest)
    {
        QuestBoardController controller = questBoard.GetComponent<QuestBoardController>();
        if (controller != null)
        {
            controller.Initialize(quest, playerInventory);
        }
    }
}
