using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClick : MonoBehaviour
{
    public Button[] Buttons; // ��ư �迭
    public FarmSystem FarmSystem; // FarmSystem ����
    public ItemObject[] seedItems; // ��ư���� �����ϴ� ���� ������ �迭

    private void Start()
    {
        // �� ��ư�� Ŭ�� �̺�Ʈ �߰�
        for (int i = 0; i < Buttons.Length; i++)
        {
            int index = i; // ���� ������ ĸó
            Buttons[i].onClick.AddListener(() => OnButtonClicked(index));
        }
    }

    /// <summary>
    /// ��ư Ŭ�� �� FarmSystem�� selectedSeed�� ����
    /// </summary>
    /// <param name="index">Ŭ���� ��ư�� �ε���</param>
    private void OnButtonClicked(int index)
    {
        if (FarmSystem != null && seedItems != null && index < seedItems.Length)
        {
            FarmSystem.selectedSeed = seedItems[index]; // ���õ� ���� ����
            Debug.Log($"���� {seedItems[index].Item_Name}�� ���õǾ����ϴ�.");
        }
        else
        {
            Debug.LogWarning("FarmSystem�̳� seedItems ������ �߸��Ǿ����ϴ�.");
        }
    }
}
