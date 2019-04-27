using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIControl;
using TMPro;

public class ChallengeModeManager : MonoBehaviour
{

    GameManager gameManager;

    //debug
    public int timeScore = 0;
    public int TimeScore { get { return timeScore; } }
    public Ball ball;
    [SerializeField]
    private GameObject wall;
    [SerializeField]
    private GameObject challengeModeScoreboard;
    [SerializeField]
    private GameObject storyModeScoreboard;


    [SerializeField]
    private TextMeshProUGUI timerText;

    public void Initialize(GameManager gameManager)
    {
        this.gameManager = gameManager;
        
    }

    public void StartChallenge()
    {
        gameManager.OnScoredGoalEvent += OnScoredGoal;
        wall.SetActive(true);
        challengeModeScoreboard.SetActive(true);
        storyModeScoreboard.SetActive(false);
        gameManager.StartTimer(timerText, false);
        gameManager.StartGame();
    }

    private void GameOver()
    {
        ReferencesHolder.Instance.UIStateManager.CloseAll();
        ReferencesHolder.Instance.UIStateManager.OpenLayout(UILayoutsIDs.ChallengeLevelFinishedLayout);
        if (ReferencesHolder.Instance.LeadersBoardManager.CheckNewHighScore(5))
        {
            Debug.Log("new score, we should show insert name for leaderboard");
            ReferencesHolder.Instance.UIStateManager.OpenLayout(UILayoutsIDs.NewHighScoreLayout, true);

        }
        gameManager.GameOver();
    }

    private void OnScoredGoal(int player)
    {
        GameOver();
    }
}
