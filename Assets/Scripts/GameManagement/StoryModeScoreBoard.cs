using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StoryModeScoreBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timerText;
    public TextMeshProUGUI TimerText { get { return timerText; } }
    [SerializeField]
    private TextMeshProUGUI player1ScoreText;
    [SerializeField]
    private TextMeshProUGUI player2ScoreText;

    private ScoreManager scoreManager;

    public void Initialize()
    {
        player1ScoreText.text = 0.ToString("00");
        player2ScoreText.text = 0.ToString("00");
        timerText.text = "0:00";
    }

    public void SetScoreDisplay(string player1, string player2)
    {
        player1ScoreText.text = player1;
        player2ScoreText.text = player2;
    }

    public void SetTimerDisplay(string time)
    {
        timerText.text = time;
    }
}
