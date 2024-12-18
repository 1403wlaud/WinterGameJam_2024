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
    private int currentDay = 1; // Day 초기값
    private const float secondsPerGameHour = 10f; // 한 시간에 10초
    private bool timerRunning = true; // 타이머 동작 상태

    private const int startAlphaTime = 14; // Alpha 증가 시작 시간 (8시 PM)
    private const int maxAlphaTime = 25; // Alpha 증가 종료 시간 (1시 AM)

    void Start()
    {
        UpdateDayDisplay(); // 초기 Day 표시

        // Alpha 조절 대상 초기화
        if (targetObject != null)
        {
            targetImage = targetObject.GetComponent<Image>();
            targetSpriteRenderer = targetObject.GetComponent<SpriteRenderer>();
        }
    }

    void Update()
    {
        if (!timerRunning) return; // 타이머가 멈췄으면 Update 중지

        // 경과 시간 업데이트
        elapsedTime += Time.deltaTime;

        // 가상 시간 증가 로직
        if (elapsedTime >= secondsPerGameHour / 60f) // 1 "가상 분"마다 실행
        {
            elapsedTime = 0f; // 초기화
            gameMinutes += 1; // 1분 추가

            if (gameMinutes >= 60) // 60분이 넘어가면
            {
                gameMinutes = 0; // 분 초기화
                gameHours += 1; // 1시간 추가

                if (gameHours > 24) // 24시가 넘으면
                {
                    gameHours = 1; // 1시로 초기화
                    timerRunning = false; // 타이머 정지
                    currentDay += 1; // Day 증가
                    UpdateDayDisplay(); // Day 업데이트
                }
            }

            // 시간 업데이트
            UpdateTimeDisplay();
        }

        UpdateAlphaValue(); // Alpha 값 업데이트
    }

    void UpdateTimeDisplay()
    {
        // 오전/오후 표시
        string period = gameHours < 12 ? "AM" : "PM";

        // 12시간제로 변환 (0시는 12시로 표시)
        int displayHours = gameHours % 12;
        if (displayHours == 0) displayHours = 12;

        // 시간 문자열 업데이트
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
    /// Alpha 값 업데이트
    /// </summary>
    private void UpdateAlphaValue()
    {
        if (targetObject == null) return;

        float alpha = 0f;

        // 8시 PM (20) 이후부터 Alpha 값 증가 시작
        if (gameHours >= startAlphaTime || (gameHours >= 0 && gameHours <= 1))
        {
            if (gameHours == 1) // 1시일 때 Alpha는 최대
            {
                alpha = 255f;
            }
            else if (gameHours >= startAlphaTime)
            {
                // Alpha 값은 (현재 시간 - 시작 시간) / (종료 시간 - 시작 시간)
                alpha = Mathf.Clamp01((float)(gameHours - startAlphaTime) / (maxAlphaTime - startAlphaTime));
            }
            else if (gameHours >= 0 && gameHours <= 1) // 자정 이후 Alpha 유지
            {
                alpha = 255f;
            }
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
    public void RestartTimer()
    {
        // 타이머를 다시 시작할 때 호출
        timerRunning = true;
        gameHours = 8; // 아침 8시로 초기화
        gameMinutes = 0; // 분 초기화
        UpdateTimeDisplay();
        timeDisplay.color = Color.black;
        txt.color = Color.black;
        dayDisplay.color = Color.black;
        Color color=targetSpriteRenderer.color;
        color.a = 0f;
    }
}
