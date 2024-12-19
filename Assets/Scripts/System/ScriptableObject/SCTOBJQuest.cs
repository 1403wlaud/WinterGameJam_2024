using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "QuestSetting")]
public class SCTOBJQuest : Quest
{
    [SerializeField] private ItemObject RewardItem;
    [SerializeField] private int ItemCount;
    [SerializeField] private ItemObject NeedObj2;
    [SerializeField] private int NeedItemCount2;
    public ItemObject Q_NeedObj2 => NeedObj2;
    public int Q_NeedItemCount2 => NeedItemCount2;
    public ItemObject Q_RewardItem =>RewardItem;
    public int Q_ItemCount => ItemCount;
}
