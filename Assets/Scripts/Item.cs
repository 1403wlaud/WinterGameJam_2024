using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    seed,
    food
}
public class Item : ScriptableObject
{
    [SerializeField] private ItemType type;
    [SerializeField] private string itemName;
    [SerializeField] private Sprite itemImage;

    public ItemType Item_Type => type;
    public string Item_Name => itemName;
    public Sprite Item_Image => itemImage;
}
