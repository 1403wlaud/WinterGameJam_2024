using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
    public GameManager gameManager;
    TextMeshProUGUI textMeshProUGUI;

    private void Start()
    {
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }
    // Update is called once per frame
    void Update()
    {
        if(gameManager.Player != null) 
            textMeshProUGUI.text=gameManager.Player.GetComponent<ItemInventory>().Money.ToString();
    }
}
