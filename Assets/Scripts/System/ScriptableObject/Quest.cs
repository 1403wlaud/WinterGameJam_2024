using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : ScriptableObject
{
    [SerializeField] private int Day;
    [SerializeField] private Item NeedObj;
    [SerializeField] private int Reward;
}
