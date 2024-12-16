using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CropsItem", menuName = "Item/Crops")]
public class CropsItem : Item
{
    [SerializeField] private int SellingPrice;

    public int sellingPrice=>SellingPrice;
}
