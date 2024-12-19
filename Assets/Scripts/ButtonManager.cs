using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public GameManager GameManager;
    private GameObject Farm;
    public GameObject Qest;
    public GameObject Store;
    public GameObject Store2;
    private GameObject PlayerInventory;
    private GameObject BackPack;
    private Button InventoryBtn;
    private Button FarmOffBtn;
    private Move pMove;
    private bool DubbleClickCheck=false;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (GameManager.Player != null)
        {
            BackPack=GameManager.Player.transform.GetChild(1).transform.GetChild(0).gameObject;
            PlayerInventory = GameManager.Player.transform.GetChild(1).transform.GetChild(1).gameObject;
            if(PlayerInventory == null) { Debug.LogWarning("°Á µÚÁö¼À"); }
            Farm=GameManager.Player.transform.GetChild(1).transform.GetChild(2).gameObject;
            FarmOffBtn=Farm.transform.GetChild(13).GetComponent<Button>();
            FarmOffBtn.onClick.AddListener(FarmExitButtonClick);
            pMove=GameManager.Player.GetComponent<Move>();
            InventoryBtn=BackPack.GetComponent<Button>();
            InventoryBtn.onClick.AddListener(InventoryOn);
        }
        else
        {
            Debug.LogWarning("GameManager.Player°¡ nullÀÔ´Ï´Ù.");
        }

    }

    public void InventoryOn()
    {
        if (DubbleClickCheck == false)
        {
            PlayerInventory.SetActive(true);
            DubbleClickCheck = true;
        }
        else
        {
            PlayerInventory.SetActive(false);
            DubbleClickCheck = false;
        }

    }

    public void FarmButtonClick()
    {
        Farm.SetActive(true);
        pMove.enabled = false;
    }
    public void FarmExitButtonClick()
    {
        Farm.SetActive(false);
        pMove.enabled = true;
    }
    public void QuestButtonClick()
    {
        Qest.SetActive(true);
        PlayerInventory.SetActive(false);
        BackPack.SetActive(false);
        pMove.enabled=false;
    }
    public void QuestButtonExitClick()
    {
        Qest.SetActive(false );
        PlayerInventory.SetActive(true);
        BackPack.SetActive(true);
        pMove.enabled=true;
    }
    public void StoreButtonClick()
    {
        Store.SetActive(true);
        Store2.SetActive(true);
        PlayerInventory.SetActive(false);
        BackPack.SetActive(false);
        pMove.enabled = false;
    }
    public void StoreExitButtonClick()
    {
        Store.SetActive(false);
        Store2.SetActive(false);
        Store.GetComponent<StoreSystem>().BuySell_Panel.SetActive(false);
        BackPack.SetActive(true);
        PlayerInventory.SetActive(true);
        pMove.enabled = true;
    }
}
