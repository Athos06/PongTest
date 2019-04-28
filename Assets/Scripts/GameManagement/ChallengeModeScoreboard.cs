using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChallengeModeScoreboard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timerText;
    public TextMeshProUGUI TimerText { get { return timerText; } }
    [SerializeField]
    private TextMeshProUGUI highScoreText;

    public void Initialize()
    {
        timerText.text = "0:00";
        HighScoreData highscore = ReferencesHolder.Instance.LeadersBoardManager.LeaderBoardList[0];
        highScoreText.text = "1: " + highscore.playerName + " " + TimeFormatHelper.GetTimeInFormat(highscore.scoreInSeconds);
    }

}
