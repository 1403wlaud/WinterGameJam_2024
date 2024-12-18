using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public int Day = 1;
    public Text timeDisplay; // �ð��� ǥ���� UI Text
    public Text txt;
    public Text dayDisplay; // Day�� ǥ���� UI Text

    public GameObject targetObject; // Alpha ���� ������ ��� ������Ʈ
    private Image targetImage; // Alpha ������ ���� Image ������Ʈ ����
    private SpriteRenderer targetSpriteRenderer; // SpriteRenderer ����

    private float elapsedTime = 0f; // ����� ���� �ð�
    private int gameHours = 8; // ���� �ð� (��ħ 8��)
    private int gameMinutes = 0; // ���� ��
    private int currentDay = 1; // Day �ʱⰪ
    private const float secondsPerGameHour = 10f; // �� �ð��� 10��
    private bool timerRunning = true; // Ÿ�̸� ���� ����

    private const int startAlphaTime = 14; // Alpha ���� ���� �ð� (8�� PM)
    private const int maxAlphaTime = 25; // Alpha ���� ���� �ð� (1�� AM)

    void Start()
    {
        UpdateDayDisplay(); // �ʱ� Day ǥ��

        // Alpha ���� ��� �ʱ�ȭ
        if (targetObject != null)
        {
            targetImage = targetObject.GetComponent<Image>();
            targetSpriteRenderer = targetObject.GetComponent<SpriteRenderer>();
        }
    }

    void Update()
    {
        if (!timerRunning) return; // Ÿ�̸Ӱ� �������� Update ����

        // ��� �ð� ������Ʈ
        elapsedTime += Time.deltaTime;

        // ���� �ð� ���� ����
        if (elapsedTime >= secondsPerGameHour / 60f) // 1 "���� ��"���� ����
        {
            elapsedTime = 0f; // �ʱ�ȭ
            gameMinutes += 1; // 1�� �߰�

            if (gameMinutes >= 60) // 60���� �Ѿ��
            {
                gameMinutes = 0; // �� �ʱ�ȭ
                gameHours += 1; // 1�ð� �߰�

                if (gameHours > 24) // 24�ð� ������
                {
                    gameHours = 1; // 1�÷� �ʱ�ȭ
                    timerRunning = false; // Ÿ�̸� ����
                    currentDay += 1; // Day ����
                    UpdateDayDisplay(); // Day ������Ʈ
                }
            }

            // �ð� ������Ʈ
            UpdateTimeDisplay();
        }

        UpdateAlphaValue(); // Alpha �� ������Ʈ
    }

    void UpdateTimeDisplay()
    {
        // ����/���� ǥ��
        string period = gameHours < 12 ? "AM" : "PM";

        // 12�ð����� ��ȯ (0�ô� 12�÷� ǥ��)
        int displayHours = gameHours % 12;
        if (displayHours == 0) displayHours = 12;

        // �ð� ���ڿ� ������Ʈ
        timeDisplay.text = string.Format("{0:00}:{1:00} {2}", displayHours, gameMinutes, period);
        timeDisplay.color = Color.white;
        txt.color = Color.white;
        dayDisplay.color = Color.white;
    }

    void UpdateDayDisplay()
    {
        dayDisplay.text = "Day " + currentDay;
    }

    /// <summary>
    /// Alpha �� ������Ʈ
    /// </summary>
    private void UpdateAlphaValue()
    {
        if (targetObject == null) return;

        float alpha = 0f;

        // 8�� PM (20) ���ĺ��� Alpha �� ���� ����
        if (gameHours >= startAlphaTime || (gameHours >= 0 && gameHours <= 1))
        {
            if (gameHours == 1) // 1���� �� Alpha�� �ִ�
            {
                alpha = 255f;
            }
            else if (gameHours >= startAlphaTime)
            {
                // Alpha ���� (���� �ð� - ���� �ð�) / (���� �ð� - ���� �ð�)
                alpha = Mathf.Clamp01((float)(gameHours - startAlphaTime) / (maxAlphaTime - startAlphaTime));
            }
            else if (gameHours >= 0 && gameHours <= 1) // ���� ���� Alpha ����
            {
                alpha = 255f;
            }
        }

        // Alpha �� ����
        if (targetImage != null)
        {
            Color color = targetImage.color;
            color.a = alpha;
            targetImage.color = color;
        }
        else if (targetSpriteRenderer != null)
        {
            Color color = targetSpriteRenderer.color;
            color.a = alpha;
            targetSpriteRenderer.color = color;
        }
    }
    public void RestartTimer()
    {
        // Ÿ�̸Ӹ� �ٽ� ������ �� ȣ��
        timerRunning = true;
        gameHours = 8; // ��ħ 8�÷� �ʱ�ȭ
        gameMinutes = 0; // �� �ʱ�ȭ
        UpdateTimeDisplay();
        timeDisplay.color = Color.black;
        txt.color = Color.black;
        dayDisplay.color = Color.black;
        Color color=targetSpriteRenderer.color;
        color.a = 0f;
    }
}
