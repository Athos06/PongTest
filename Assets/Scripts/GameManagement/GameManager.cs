using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIControl;
using System;
using TMPro;

public class GameManager : MonoBehaviour
{

    public enum GameModes
    {
        Story,
        Challenge
    }

    public Action<int> OnScoredGoalEvent;
    public Action OnCountdownFinishedEvent;
    public Action OnGameStarted;

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
    private StartGameCountdownController startGameCountdown;

    private IGameMode gameModeActive;

    [SerializeField]
    private int RoundTime = 10;
    [SerializeField]
    private Vector3 ballStartPosition;

    private Ball ball;
    private CharacterController player1;
    private CharacterController player2;

    public Ball GetActiveBall()
    {
        return ball;
    }

    public void SetPlayers(CharacterController player1, CharacterController player2)
    {
        this.player1 = player1;
        this.player2 = player2;
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

    //public void StartStoryMode()
    //{
    //    storyModeManager.StartNextLevel();
    //}

    //public void StartChallengeMode()
    //{
    //    challengeModeManager.StartChallenge();
    //}

    public int GetChallengeScore()
    {
        return challengeModeManager.TimeScore;
    }

    public void StartTimer(TextMeshProUGUI timerText, bool IsCountDown)
    {
        if (IsCountDown)
            timerCountdown.StartCountdown(timerText, RoundTime);
        else
        {
            timerCountdown.StartTimer(timerText);
        }
    }

    public void StartGameMode(GameModes gameMode)
    {
        if (ball == null)
        {
            ball = Instantiate(ballPrefab);
            ball.name = "ball";
            ball.Initialize(ballStartPosition);
            ball.gameObject.SetActive(false);
        }

        switch (gameMode)
        {
            case GameModes.Story:
                gameModeActive = storyModeManager;
                //ReferencesHolder.Instance.ScreenFader.StartFadeOut(storyModeManager.StartNextLevel);
                break;
            case GameModes.Challenge:
                gameModeActive = challengeModeManager;
                //ReferencesHolder.Instance.ScreenFader.StartFadeOut(challengeModeManager.StartChallenge);
                break;
        }

        ReferencesHolder.Instance.ScreenFader.StartFadeOut(gameModeActive.StartGameMode);

    }

    public void StartGame()
    {
        ReferencesHolder.Instance.CamerasController.SetGameCameraCamera();
        StartCoroutine(StartingGame());

        //timerCountdown.StartCountdown(20);
        //GameObject HumanPlayer = Instantiate(HumanPlayerPrefab);
        //skillHudController.Initialize(HumanPlayer);
    }

    private float countdown = 3.0f;
    private IEnumerator StartingGame()
    {
        ReferencesHolder.Instance.UIStateManager.OpenPanel(UIPanelsIDs.StartCountDownPanel);

        float time = countdown;
        while(time >= 0)
        {
            time -= Time.deltaTime;
            startGameCountdown.CountdownText.text = ((int)time + 1).ToString();
            yield return null;
        }

        ReferencesHolder.Instance.CamerasController.SetGameCameraCamera();
        ReferencesHolder.Instance.UIStateManager.ClosePanel(UIPanelsIDs.StartCountDownPanel);

        ball.LaunchBall();

        if (OnGameStarted != null)
            OnGameStarted.Invoke();

        yield return null;
    }

    public void GameOver()
    {
        ball.DisableBall();
        timerCountdown.StopTimer();

    }

    public void FinishGame()
    {
        if (ball != null)
        {
            Destroy(ball.gameObject);
            ball = null;
        }
        ReferencesHolder.Instance.ScreenFader.StartFadeOut(gameModeActive.FinishGameMode);
        scoreManager.ResetScore();
    }

    public void RestartGame()
    {
        if (ball != null)
        {
            Destroy(ball.gameObject);
            ball = null;
        }
        ReferencesHolder.Instance.ScreenFader.StartFadeOut(gameModeActive.RestartGameMode);
        scoreManager.ResetScore();
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
