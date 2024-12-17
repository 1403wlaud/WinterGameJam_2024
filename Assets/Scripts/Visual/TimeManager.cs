using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public int Day = 1;
    public Text timeDisplay; // 시간을 표시할 UI Text
    public Text txt;
    public Text dayDisplay;  // Day를 표시할 UI Text

    private float elapsedTime = 0f; // 경과한 실제 시간
    private int gameHours = 8; // 시작 시간 (아침 8시)
    private int gameMinutes = 0; // 시작 분
    private int currentDay = 1; // Day 초기값
    private const float secondsPerGameHour = 10f; // 한 시간에 30초
    private bool timerRunning = true; // 타이머 동작 상태

    private bool isBlinking = false; // 깜빡임 제어

    void Start()
    {
        StartCoroutine(BlinkTimeText()); // 깜빡거림 효과 시작
        UpdateDayDisplay(); // 초기 Day 표시
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

                if (gameHours > 23) // 24시가 넘으면
                {
                    gameHours = 0; // 0시로 초기화
                }

                // 오전 1시가 되면 타이머 멈추고 Day 증가
                if (gameHours == 1 && gameMinutes == 0)
                {
                    timerRunning = false; // 타이머 정지
                    currentDay += 1; // Day 증가
                    UpdateDayDisplay(); // Day 업데이트
                }
            }

            // 시간 업데이트
            UpdateTimeDisplay();
        }
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
    }

    void UpdateDayDisplay()
    {
        dayDisplay.text = "Day " + currentDay;
    }

    public void RestartTimer()
    {
        // 타이머를 다시 시작할 때 호출
        timerRunning = true;
        gameHours = 8; // 아침 8시로 초기화
        gameMinutes = 0; // 분 초기화
        UpdateTimeDisplay();
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
}
