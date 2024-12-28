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
    public GameObject Note;
    private GameObject PlayerInventory;
    private GameObject BackPack;
    private Button InventoryBtn;
    private Button FarmOffBtn;
    private Move pMove;
    private bool DubbleClickCheck=false;
    private bool isInitialized = false;

    public AudioClip[] audioClips;
    public AudioSource BGM;

    private void Update()
    {
        if (!isInitialized && GameManager.Player != null) // 한번만 실행
        {
            InitializeComponents();
            isInitialized = true;
        }
    }

    private void InitializeComponents()
    {
        // GameManager.Player가 null이 아니면 필요한 컴포넌트를 초기화
        BackPack = GameManager.Player.transform.GetChild(1).transform.GetChild(0).gameObject;
        PlayerInventory = GameManager.Player.transform.GetChild(1).transform.GetChild(1).gameObject;
        Farm = GameManager.Player.transform.GetChild(1).transform.GetChild(2).gameObject;

        if (PlayerInventory == null || Farm == null || BackPack == null)
        {
            Debug.LogWarning("필요한 컴포넌트가 초기화되지 않았습니다.");
            return;
        }

        InventoryBtn = BackPack.GetComponent<Button>();
        FarmOffBtn = Farm.transform.GetChild(13).GetComponent<Button>();
        pMove = GameManager.Player.GetComponent<Move>();

        if (InventoryBtn != null)
        {
            InventoryBtn.onClick.AddListener(InventoryOn);
        }

        if (FarmOffBtn != null)
        {
            FarmOffBtn.onClick.AddListener(FarmExitButtonClick);
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
    public void NoteBtnOn()
    {
        Note.SetActive(true);
        pMove.enabled = false;
        PlayerInventory.SetActive(false);
        BackPack.SetActive(false);
    }

    public void NoteExitBtn()
    {
        Note.SetActive(false);
        pMove.enabled = true;
        BackPack.SetActive(true);
    }

    public void FarmButtonClick()
    {
        Farm.SetActive(true);
        pMove.enabled = false;
        BGM.clip=audioClips[1];
        BGM.Play();
    }
    public void FarmExitButtonClick()
    {
        Farm.SetActive(false);
        pMove.enabled = true;
        BGM.clip=audioClips[0];
        BGM.Play();
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
        pMove.enabled = true;
    }
}
