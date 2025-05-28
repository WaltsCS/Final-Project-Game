using System;
using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float timeLimit = 60f;
    private bool isTimerActive = true;
    private float timeRemaining;
    private LevelManager levelManager;
    private PlayerStates playerStates;

    void Awake()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        playerStates = GameObject.Find("Player").GetComponent<PlayerStates>();
        timeRemaining = timeLimit;
    }

    void Update()
    {
        if (isTimerActive && levelManager.IsLevelActive)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0)
            {
                timeRemaining = 0;  // Clamp to zero just in case
                isTimerActive = false;
                UpdateTimerText();  // Update the timer to 0
                playerStates.Die.Invoke();
            }
            else
            {
                UpdateTimerText();
            }
        }
    }

    private void UpdateTimerText()
    {
        timerText.text = $"{Mathf.CeilToInt(timeRemaining)}";
    }

    public void StopTimer()
    {
        if (isTimerActive)
        {
            isTimerActive = false;
        }
        timerText.gameObject.SetActive(false);
    }

}
