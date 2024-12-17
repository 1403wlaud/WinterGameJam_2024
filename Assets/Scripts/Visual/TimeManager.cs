using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public int Day = 1;
    public Text timeDisplay; // �ð��� ǥ���� UI Text
    public Text txt;
    public Text dayDisplay;  // Day�� ǥ���� UI Text

    private float elapsedTime = 0f; // ����� ���� �ð�
    private int gameHours = 8; // ���� �ð� (��ħ 8��)
    private int gameMinutes = 0; // ���� ��
    private int currentDay = 1; // Day �ʱⰪ
    private const float secondsPerGameHour = 10f; // �� �ð��� 30��
    private bool timerRunning = true; // Ÿ�̸� ���� ����

    private bool isBlinking = false; // ������ ����

    void Start()
    {
        StartCoroutine(BlinkTimeText()); // �����Ÿ� ȿ�� ����
        UpdateDayDisplay(); // �ʱ� Day ǥ��
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

                if (gameHours > 23) // 24�ð� ������
                {
                    gameHours = 0; // 0�÷� �ʱ�ȭ
                }

                // ���� 1�ð� �Ǹ� Ÿ�̸� ���߰� Day ����
                if (gameHours == 1 && gameMinutes == 0)
                {
                    timerRunning = false; // Ÿ�̸� ����
                    currentDay += 1; // Day ����
                    UpdateDayDisplay(); // Day ������Ʈ
                }
            }

            // �ð� ������Ʈ
            UpdateTimeDisplay();
        }
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
    }

    void UpdateDayDisplay()
    {
        dayDisplay.text = "Day " + currentDay;
    }

    public void RestartTimer()
    {
        // Ÿ�̸Ӹ� �ٽ� ������ �� ȣ��
        timerRunning = true;
        gameHours = 8; // ��ħ 8�÷� �ʱ�ȭ
        gameMinutes = 0; // �� �ʱ�ȭ
        UpdateTimeDisplay();
    }

    IEnumerator BlinkTimeText()
    {
        while (true)
        {
            if (isBlinking)
            {
                // �����Ÿ�: �����ϰ� �����
                txt.color = new Color(timeDisplay.color.r, timeDisplay.color.g, timeDisplay.color.b, 0);
            }
            else
            {
                // �����Ÿ�: ���� ������ ����
                txt.color = new Color(timeDisplay.color.r, timeDisplay.color.g, timeDisplay.color.b, 1);
            }

            isBlinking = !isBlinking; // ���� ��ȯ
            yield return new WaitForSeconds(0.25f); // 0.5�ʸ��� �����Ÿ�
        }
    }
}
