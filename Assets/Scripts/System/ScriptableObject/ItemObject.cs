using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ItemSetting")]
public class ItemObject : Item
{
    [SerializeField] private int TimeTaken;
    [SerializeField] private int SellingPrice;

    public int sellingPrice => SellingPrice;
    public int Seed_TimeTaken=>TimeTaken;
}
