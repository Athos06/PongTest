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
    public Action OnGameOver;
    public Action<bool> OnGamePause;

    public bool IsGamePause { get; private set; }
    public bool IsGamePlaying { get; private set; }

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
    public TimerCountdown TimerCountdown { get { return timerCountdown; } }
    [SerializeField]
    private SkillHudController skillHudController;
    public SkillHudController SKillHudController { get { return skillHudController; } }
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

    private float countdown = 3.0f;

    [SerializeField]
    private int RoundTime = 10;
    [SerializeField]
    private Vector3 ballStartPosition;

    private SavingManager savingManager;
    public SavingManager SavingManager { get { return savingManager; } }

    private Ball ball;
    private Coroutine StartingGameCoroutine;

    public Ball GetActiveBall()
    {
        return ball;
    }

    public void Initialize()
    {
        savingManager = new SavingManager();
        savingManager.Initialize();
        storyModeManager.Initialize(this);
        challengeModeManager.Initialize(this);

        goalPlayer1.OnScoredGoal += OnScoreGoal;
        goalPlayer2.OnScoredGoal += OnScoreGoal;
        scoreManager.Initialize();
        timerCountdown.OnCountdownFinished += OnCountdownFinished;

        ReferencesHolder.Instance.UIStateManager.OpenLayout(UILayoutsIDs.MainMenuLayout);

        //StartGame();
    }


    public void PauseGame()
    {
        if (IsGamePlaying)
        {
            Time.timeScale = 0;
            IsGamePause = true;
            UIStateManager.Instance.OpenLayout(UILayoutsIDs.PauseLayout, true);
            if (OnGamePause != null)
                OnGamePause.Invoke(true);
        }
    }

    public void UnPauseGame()
    {
        if (IsGamePlaying)
        {
            Time.timeScale = 1;
            IsGamePause = false;
            UIStateManager.Instance.CloseLastState();
            if (OnGamePause != null)
                OnGamePause.Invoke(false);
        }
    }

    public void QuitGame()
    {
        IsGamePlaying = false;

        if (StartingGameCoroutine != null)
        {
            StopCoroutine(StartingGameCoroutine);
            StartingGameCoroutine = null;
        }

        timerCountdown.StopTimer();
        GameOver();

        UnPauseGame();

        ReferencesHolder.Instance.UIStateManager.ClosePanel(UIPanelsIDs.StartCountDownPanel);
        ReferencesHolder.Instance.ScreenFader.StartFadeOut(gameModeActive.FinishGameMode);

        if (ball != null)
        {
            Destroy(ball.gameObject);
            ball = null;
        }

        scoreManager.ResetScore();
    }

    public int GetChallengeScore()
    {
        return TimerCountdown.TimerCurrentTime;
    }

    public int[] GetScore()
    {
        return scoreManager.PlayersScore;
    }

    public int GetWinnerPlayer()
    {
        if (scoreManager.PlayersScore[0] > scoreManager.PlayersScore[1])
        {
            return 0;
        }
        if (scoreManager.PlayersScore[0] < scoreManager.PlayersScore[1])
        {
            return 1;
        }
        //draw
        return -1;
    }



    public string GetLevelDescription()
    {
        return gameModeActive.GetLevelDescription();
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
                break;
            case GameModes.Challenge:
                gameModeActive = challengeModeManager;
                break;
        }

        ReferencesHolder.Instance.ScreenFader.StartFadeOut(gameModeActive.StartGameMode);
    }

    public void StartGame()
    {
        IsGamePlaying = true;
        ReferencesHolder.Instance.CamerasController.SetGameCameraCamera();
        StartingGameCoroutine = StartCoroutine(StartingGame());
    }

    private IEnumerator StartingGame()
    {
        ReferencesHolder.Instance.UIStateManager.OpenPanel(UIPanelsIDs.StartCountDownPanel);

        float time = countdown;
        while (time >= 0)
        {
            time -= Time.deltaTime;
            startGameCountdown.CountdownText.text = ((int)time + 1).ToString();
            yield return null;
        }

        ReferencesHolder.Instance.CamerasController.SetGameCameraCamera();
        ReferencesHolder.Instance.UIStateManager.ClosePanel(UIPanelsIDs.StartCountDownPanel);
        ReferencesHolder.Instance.UIStateManager.OpenLayout(UILayoutsIDs.HUDLayout);

        ball.LaunchBall();

        if (OnGameStarted != null)
            OnGameStarted.Invoke();

        yield return null;
    }

    public void GameOver()
    {
        ball.DisableBall();
        timerCountdown.StopTimer();

        if (OnGameOver != null)
            OnGameOver.Invoke();

    }

    public void FinishGame()
    {
        IsGamePlaying = false;

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
    }


}
