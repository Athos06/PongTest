using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIControl;
using System;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Action<int> OnScoredGoalEvent;
    public Action OnCountdownFinishedEvent;

    [SerializeField]
    private Ball ballPrefab;
    [SerializeField]
    private GoalsController goalPlayer1;
    [SerializeField]
    private GoalsController goalPlayer2;
    [SerializeField]
    private ScoreManager scoreManager;
    [SerializeField]
    private TimerCountdown timerCountdown;
    [SerializeField]
    private SkillHudController skillHudController;
    [SerializeField]
    private GameObject HumanPlayerPrefab;
    [SerializeField]
    private GameObject AIPlayerPrefab;

    [SerializeField]
    private StoryModeManager storyModeManager;
    [SerializeField]
    private ChallengeModeManager challengeModeManager;

    [SerializeField]
    private Vector3 ballStartPosition;

    private Ball ball;

    public Ball GetActiveBall()
    {
        if (ball == null)
        {
            ball = Instantiate(ballPrefab);
            ball.name = "ball";
            ball.Initialize(ballStartPosition);
        }
        return ball;
    }

    public void Initialize()
    {

        storyModeManager.Initialize(this);
        challengeModeManager.Initialize(this);

        goalPlayer1.OnScoredGoal += OnScoreGoal;
        goalPlayer2.OnScoredGoal += OnScoreGoal;
        scoreManager.Initialize();
        timerCountdown.OnCountdownFinished += OnCountdownFinished;

        ReferencesHolder.Instance.UIStateManager.OpenLayout(UILayoutsIDs.MainMenuLayout);

        //StartGame();
    }

    public void StartStoryMode()
    {
        storyModeManager.StartNextLevel();
    }

    public void StartChallengeMode()
    {
        challengeModeManager.StartChallenge();
    }

    public int GetChallengeScore()
    {
        return challengeModeManager.TimeScore;
    }

    public void StartTimer(TextMeshProUGUI timerText, bool IsCountDown)
    {
        if(IsCountDown)
            timerCountdown.StartCountdown(timerText, 20);
        else
        {
            timerCountdown.StartTimer(timerText);
        }
    }

    public void StartGame()
    {
        if (ball == null)
        {
            ball = Instantiate(ballPrefab);
            ball.name = "ball";
            ball.Initialize(ballStartPosition);
        }

        //timerCountdown.StartCountdown(20);
        GameObject HumanPlayer = Instantiate(HumanPlayerPrefab);
        //Instantiate(AIPlayerPrefab);
        skillHudController.Initialize(HumanPlayer);
    }

    public void GameOver()
    {
        ball.DisableBall();
       

    }

    private void OnScoreGoal(int player)
    {
        
        scoreManager.UpdateScoreGoal(player, 1);
        ball.GoalScored();
        ball.RespawnBall();

        if (OnScoredGoalEvent != null)
            OnScoredGoalEvent.Invoke(player);

    }

    private void OnCountdownFinished()
    {
        if (OnCountdownFinishedEvent != null)
            OnCountdownFinishedEvent.Invoke();

        //GameOver();
    }
}
