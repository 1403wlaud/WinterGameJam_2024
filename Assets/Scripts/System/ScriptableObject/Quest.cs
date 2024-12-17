using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : ScriptableObject
{
    [SerializeField] private int Day;
    [SerializeField] private Item NeedObj;
    [SerializeField] private int NeedItemCount;
    [SerializeField] private int RewardMoney;
    [SerializeField] private Sprite CharacterFaceImg;
    [SerializeField] private string CharacterName;
    [SerializeField] private bool CheckQuest;

    public int Q_Day => Day;
    public Item Q_NeedObj => NeedObj;
    public int Q_RewardMoney => RewardMoney;
    public int Q_NeedItemCount=>NeedItemCount;     
    public Sprite Q_CharacterFaceImg => CharacterFaceImg;
    public string Q_CharacterName => CharacterName;
    public bool Q_CheckQuest => CheckQuest;
}
