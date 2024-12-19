using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public int Day = 1;
    public Text timeDisplay; // 시간을 표시할 UI Text
    public Text txt;
    public Text dayDisplay; // Day를 표시할 UI Text

    public GameObject targetObject; // Alpha 값을 변경할 대상 오브젝트
    private Image targetImage; // Alpha 변경을 위한 Image 컴포넌트 참조
    private SpriteRenderer targetSpriteRenderer; // SpriteRenderer 참조

    private float elapsedTime = 0f; // 경과한 실제 시간
    private int gameHours = 8; // 시작 시간 (아침 8시)
    private int gameMinutes = 0; // 시작 분
    private const float secondsPerGameHour = 10f; // 한 시간에 10초
    private bool timerRunning = true; // 타이머 동작 상태

    private const int eveningStart = 17; // 오후 4시 (알파값 증가 시작)
    private const int eveningEnd = 22; // 오후 10시 (알파값 증가 종료)
    private const int morningStart = 5; // 오전 5시 (알파값 감소 시작)
    private const int morningEnd = 8; // 오전 8시 (알파값 감소 종료)
    private const float maxAlpha = 0.7f; // 알파 값 최대 (0~1 사이)

    private bool isBlinking = false;

    private void Start()
    {
        UpdateDayDisplay(); // 초기 Day 표시
        StartCoroutine(BlinkTimeText());

        // Alpha 조절 대상 초기화
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
                // 깜빡거림: 투명하게 만들기
                txt.color = new Color(timeDisplay.color.r, timeDisplay.color.g, timeDisplay.color.b, 0);
            }
            else
            {
                // 깜빡거림: 원래 색으로 복원
                txt.color = new Color(timeDisplay.color.r, timeDisplay.color.g, timeDisplay.color.b, 1);
            }

            isBlinking = !isBlinking; // 상태 전환
            yield return new WaitForSeconds(0.25f); // 0.5초마다 깜빡거림
        }
    }

    private void Update()
    {
        if (!timerRunning) return;

        // 경과 시간 업데이트
        elapsedTime += Time.deltaTime;

        // 가상 시간 증가 로직
        if (elapsedTime >= secondsPerGameHour / 60f) // 1 "가상 분"마다 실행
        {
            elapsedTime = 0f;
            gameMinutes++;

            if (gameMinutes >= 60) // 60분이 넘어가면
            {
                gameMinutes = 0;
                gameHours++;

                if (gameHours >= 24) // 24시가 넘어가면
                {
                    gameHours = 0;
                    Day++;
                    UpdateDayDisplay(); // Day 증가
                }
            }

            UpdateTimeDisplay(); // 시간 업데이트
        }

        UpdateAlphaValue(); // Alpha 값 업데이트
        UpdateTextColor(); // 텍스트 색상 업데이트
    }

    private void UpdateTimeDisplay()
    {
        string period = gameHours < 12 ? "AM" : "PM";
        int displayHours = gameHours % 12;
        if (displayHours == 0) displayHours = 12;

        timeDisplay.text = string.Format("{0:00} {1:00} {2}", displayHours, gameMinutes, period);
    }

    private void UpdateDayDisplay()
    {
        dayDisplay.text = "Day " + Day;
    }

    /// <summary>
    /// Alpha 값 업데이트
    /// </summary>
    private void UpdateAlphaValue()
    {
        if (targetObject == null) return;

        float alpha = 0f;

        if (gameHours >= eveningStart && gameHours <= eveningEnd)
        {
            // 오후 4시부터 오후 10시까지 Alpha 값 증가
            alpha = Mathf.InverseLerp(eveningStart, eveningEnd, gameHours) * maxAlpha;
        }
        else if (gameHours > eveningEnd || gameHours < morningStart)
        {
            // 오후 10시 이후 ~ 오전 5시까지 Alpha 값 유지
            alpha = maxAlpha;
        }
        else if (gameHours >= morningStart && gameHours < morningEnd)
        {
            // 오전 5시부터 오전 8시까지 Alpha 값 감소
            alpha = Mathf.InverseLerp(morningEnd, morningStart, gameHours) * maxAlpha;
            alpha = maxAlpha - alpha; // 감소
        }

        // Alpha 값 적용
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
            // 오후 4시 이후 ~ 오전 5시까지 텍스트 하얗게
            timeDisplay.color = Color.white;
            txt.color = Color.white;
            dayDisplay.color = Color.white;
        }
        else
        {
            // 오전 5시 이후 텍스트 검정색
            timeDisplay.color = Color.black;
            txt.color = Color.black;
            dayDisplay.color = Color.black;
        }
    }

    public void RestartTimer()
    {
        timerRunning = true;
        gameHours = 8; // 아침 8시로 초기화
        gameMinutes = 0; // 분 초기화
        UpdateTimeDisplay();
    }
}
