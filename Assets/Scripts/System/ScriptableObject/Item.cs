using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    seed,//¾¾¾Ñ
    food,//À½½Ä
    crops//³óÀÛ¹°
}
public enum ItemCrops
{
    potato,
    corn,
    carrot,
    blueberry,
    spinach
}
public class Item : ScriptableObject
{
    [SerializeField] private ItemCrops crops;
    [SerializeField] private ItemType type;
    [SerializeField] private string itemName;
    [SerializeField] private Sprite itemImage;
    [SerializeField] private int ItemPrice;
    [SerializeField] private string ItemExample;

    public ItemCrops Item_Crops => crops;
    public string Item_Example => ItemExample;
    public int Item_Price => ItemPrice;
    public ItemType Item_Type => type;
    public string Item_Name => itemName;
    public Sprite Item_Image => itemImage;
}
