using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject Player;

    private void Update()
    {
        Player = GameObject.Find("Player(Clone)");
    }
}
