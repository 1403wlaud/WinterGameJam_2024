using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ItemSetting")]
public class ItemObject : Item
{
    [SerializeField] private int TimeTaken;
    [SerializeField] private int SellingPrice;
    [SerializeField] private Sprite[] GrrowImg;
    public int[] Seed_GrowthDurations; // 각 성장 단계의 지속 시간

    public int sellingPrice => SellingPrice;
    public int Seed_TimeTaken=>TimeTaken;
    public Sprite[] Seed_GrrowSprites=>GrrowImg;
}
