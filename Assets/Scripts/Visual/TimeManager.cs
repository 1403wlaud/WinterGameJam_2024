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
    private const float secondsPerGameHour = 5f; // �� �ð��� 10��
    private bool timerRunning = true; // Ÿ�̸� ���� ����

    private const int eveningStart = 17; // ���� 4�� (���İ� ���� ����)
    private const int eveningEnd = 22; // ���� 10�� (���İ� ���� ����)
    private const int morningStart = 5; // ���� 5�� (���İ� ���� ����)
    private const int morningEnd = 8; // ���� 8�� (���İ� ���� ����)
    private const float maxAlpha = 0.7f; // ���� �� �ִ� (0~1 ����)

    private bool isBlinking = false;

    private void Start()
    {
        UpdateDayDisplay(); // �ʱ� Day ǥ��
        StartCoroutine(BlinkTimeText());

        // Alpha ���� ��� �ʱ�ȭ
        if (targetObject != null)
        {
            targetImage = targetObject.GetComponent<Image>();
            targetSpriteRenderer = targetObject.GetComponent<SpriteRenderer>();
        }
    }
    IEnumerator BlinkTimeText()
    {
        while (true)
        {
            if (isBlinking)
            {
                // �����Ÿ�: �����ϰ� �����
                txt.gameObject.SetActive(false);
            }
            else
            {
                // �����Ÿ�: ���� ������ ����
                txt.gameObject.SetActive(true);
            }

            isBlinking = !isBlinking; // ���� ��ȯ
            yield return new WaitForSecondsRealtime(0.25f); // 0.5�ʸ��� �����Ÿ�
        }
    }

    private void Update()
    {
        if (!timerRunning) return;

        // ��� �ð� ������Ʈ
        elapsedTime += Time.deltaTime;

        // ���� �ð� ���� ����
        if (elapsedTime >= secondsPerGameHour / 60f) // 1 "���� ��"���� ����
        {
            elapsedTime = 0f;
            gameMinutes++;

            if (gameMinutes >= 60) // 60���� �Ѿ��
            {
                gameMinutes = 0;
                gameHours++;

                if (gameHours >= 24) // 24�ð� �Ѿ��
                {
                    gameHours = 0;
                    Day++;
                    UpdateDayDisplay(); // Day ����
                }
            }

            UpdateTimeDisplay(); // �ð� ������Ʈ
        }

        UpdateAlphaValue(); // Alpha �� ������Ʈ
        UpdateTextColor(); // �ؽ�Ʈ ���� ������Ʈ
    }

    private void UpdateTimeDisplay()
    {
        string period = gameHours < 12 ? "AM" : "PM";
        int displayHours = gameHours % 12;
        if (displayHours == 0) displayHours = 12;

        timeDisplay.text = string.Format("{0:00}  {1:00} {2}", displayHours, gameMinutes, period);
    }

    private void UpdateDayDisplay()
    {
        dayDisplay.text = "Day " + Day;
    }

    /// <summary>
    /// Alpha �� ������Ʈ
    /// </summary>
    private void UpdateAlphaValue()
    {
        if (targetObject == null) return;

        float alpha = 0f;

        if (gameHours >= eveningStart && gameHours <= eveningEnd)
        {
            // ���� 4�ú��� ���� 10�ñ��� Alpha �� ����
            alpha = Mathf.InverseLerp(eveningStart, eveningEnd, gameHours) * maxAlpha;
        }
        else if (gameHours > eveningEnd || gameHours < morningStart)
        {
            // ���� 10�� ���� ~ ���� 5�ñ��� Alpha �� ����
            alpha = maxAlpha;
        }
        else if (gameHours >= morningStart && gameHours < morningEnd)
        {
            // ���� 5�ú��� ���� 8�ñ��� Alpha �� ����
            alpha = Mathf.InverseLerp(morningEnd, morningStart, gameHours) * maxAlpha;
            alpha = maxAlpha - alpha; // ����
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

    private void UpdateTextColor()
    {
        if (gameHours >= eveningStart || gameHours < morningStart)
        {
            // ���� 4�� ���� ~ ���� 5�ñ��� �ؽ�Ʈ �Ͼ��
            timeDisplay.color = new Color(255,255,255);
            txt.color = new Color(255, 255, 255);
            dayDisplay.color = new Color(255, 255, 255);
        }
        else
        {
            // ���� 5�� ���� �ؽ�Ʈ ������
            timeDisplay.color = new Color(0, 0, 0);
            txt.color = new Color(0, 0, 0);
            dayDisplay.color = new Color(0, 0, 0);
        }
    }

    public void RestartTimer()
    {
        timerRunning = true;
        gameHours = 8; // ��ħ 8�÷� �ʱ�ȭ
        gameMinutes = 0; // �� �ʱ�ȭ
        UpdateTimeDisplay();
    }
}
