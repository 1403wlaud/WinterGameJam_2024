using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "QuestSetting")]
public class SCTOBJQuest : Quest
{
    [SerializeField] private Item RewardItem;
    [SerializeField] private int ItemCount;
    [SerializeField] private Item NeedObj2;
    [SerializeField] private int NeedItemCount2;
    public Item Q_NeedObj2 => NeedObj2;
    public int Q_NeedItemCount2 => NeedItemCount2;
    public Item Q_RewardItem=>RewardItem;
    public int Q_ItemCount => ItemCount;
}
