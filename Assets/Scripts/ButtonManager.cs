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
    private Button FarmOffBtn;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (GameManager.Player != null)
        {
            PlayerInventory = GameManager.Player.transform.GetChild(1).transform.GetChild(0).gameObject;
            if(PlayerInventory == null) { Debug.LogWarning("°Á µÚÁö¼À"); }
            Farm=GameManager.Player.transform.GetChild(1).transform.GetChild(1).gameObject;
            FarmOffBtn=Farm.transform.GetChild(13).GetComponent<Button>();
            FarmOffBtn.onClick.AddListener(FarmExitButtonClick);
        }
        else
        {
            Debug.LogWarning("GameManager.Player°¡ nullÀÔ´Ï´Ù.");
        }

    }

    public void FarmButtonClick()
    {
        Farm.SetActive(true);
    }
    public void FarmExitButtonClick()
    {
        Farm.SetActive(false);
    }
    public void QuestButtonClick()
    {
        Qest.SetActive(true);
        PlayerInventory.SetActive(false);
    }
    public void QuestButtonExitClick()
    {
        Qest.SetActive(false );
        PlayerInventory.SetActive(true);
    }
    public void StoreButtonClick()
    {
        Store.SetActive(true);
        Store2.SetActive(true);
        PlayerInventory.SetActive(false);
    }
    public void StoreExitButtonClick()
    {
        Store.SetActive(false);
        Store2.SetActive(false);
        Store.GetComponent<StoreSystem>().BuySell_Panel.SetActive(false);
        PlayerInventory.SetActive(true);
    }
}
