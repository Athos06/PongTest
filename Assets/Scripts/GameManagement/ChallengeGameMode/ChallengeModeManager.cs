using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIControl;
using TMPro;

public class ChallengeModeManager : MonoBehaviour, IGameMode
{

    
    public int TimeScore { get; private set; }
    [Header("Game elements")]
    [SerializeField]
    private GameObject wall;
    [SerializeField]
    private GameObject challengeModeScoreboard;
    [SerializeField]
    private GameObject storyModeScoreboard;
    [Header("Settings")]
    [SerializeField]
    private float introLength = 3.0f;
    [SerializeField]
    private Vector3 ballStartPosition;
    [SerializeField]
    private float difficultyIncrease = 0.25f;
    [SerializeField]
    private int countdown = 3;
    [TextArea, SerializeField]
    private string levelDescription;
    [Header("References")]    
    [SerializeField]
    private ChallengeModeScoreboard scoreBoard;
    [SerializeField]
    private ChallengeDifficultyController difficultyController;
    [SerializeField]
    private TimerCountdown timerCountdown;
    public TimerCountdown TimerCountdown { get { return timerCountdown; } }
    [SerializeField]
    private StartGameCountdownController startGameCountdown;
    [SerializeField]
    private GoalsManager goalsManager;
    [SerializeField]
    private BallManager ballManager;
    public BallManager BallManager { get { return ballManager; } }

    private GameManager gameManager;
    private CharacterController player1;
    private bool subcribedToEvents = false;

    public void Initialize(GameManager gameManager)
    {
        this.gameManager = gameManager;
        difficultyController.Initialize(this);
        goalsManager.Initialize();
    }


    public void StartGameMode()
    {
        if (!subcribedToEvents)
        {
            subcribedToEvents = true;
            goalsManager.OnScoredGoal += OnScoredGoal;
            gameManager.OnGameStarted += OnGameStarted;

        }

        scoreBoard.Initialize();
        wall.SetActive(true);
        challengeModeScoreboard.SetActive(true);
        storyModeScoreboard.SetActive(false);
        player1 = ReferencesHolder.Instance.CharacterFactory.CreateCharacter(CharacterFactory.CharactersType.Player);
        player1.Intialize();
        player1.SetInput(new PlayerInput());
        gameManager.SKillHudController.Initialize(player1.GetComponent<CharacterSkillController>());
        ballManager.Initialize();
        ReferencesHolder.Instance.CamerasController.SetEnemyIntroCamera();
        ReferencesHolder.Instance.ScreenFader.StartFadeIn(StartPlay);

    }

    private void StartPlay()
    {
        StartCoroutine(StartPlayCoroutine());
    }

    private IEnumerator StartPlayCoroutine()
    {
        ReferencesHolder.Instance.UIStateManager.OpenPanel(UIPanelsIDs.EnemyNamePanel);
        yield return new WaitForSeconds(introLength);
        ReferencesHolder.Instance.UIStateManager.ClosePanel(UIPanelsIDs.EnemyNamePanel);
        ballManager.SpawnBall(ballStartPosition);

        //start countdown
        ReferencesHolder.Instance.CamerasController.SetGameCameraCamera();
        ReferencesHolder.Instance.UIStateManager.OpenPanel(UIPanelsIDs.StartCountDownPanel);

        float time = countdown;
        while (time >= 0)
        {
            time -= Time.deltaTime;
            startGameCountdown.DisplayCountdownText(((int)time + 1).ToString());
            yield return null;
        }

        ReferencesHolder.Instance.CamerasController.SetGameCameraCamera();
        ReferencesHolder.Instance.UIStateManager.ClosePanel(UIPanelsIDs.StartCountDownPanel);
        ReferencesHolder.Instance.UIStateManager.OpenLayout(UILayoutsIDs.HUDLayout);

        gameManager.StartGame();
        difficultyController.StartDiffiultyIncreasing();
        yield return null;
    }

    public int GetChallengeScore()
    {
        return timerCountdown.TimerCurrentTime;
    }

    public void GameModeOver()
    {
        ballManager.DisableBall();
        timerCountdown.StopTimer();
        difficultyController.Stop();
        int score = timerCountdown.TimerCurrentTime;
        ReferencesHolder.Instance.UIStateManager.CloseAll();
        ReferencesHolder.Instance.UIStateManager.OpenLayout(UILayoutsIDs.ChallengeLevelFinishedLayout);

        if (ReferencesHolder.Instance.LeadersBoardManager.CheckNewHighScore(score) > -1)
        {
            ReferencesHolder.Instance.UIStateManager.OpenLayout(UILayoutsIDs.NewHighScoreLayout, true);
        }

        gameManager.GameOver();
    }

    public void FinishGameMode()
    {
        if (player1 != null)
        {
            Destroy(player1.gameObject);
            player1 = null;
        }

        if (subcribedToEvents)
        {
            subcribedToEvents = false;
            goalsManager.OnScoredGoal -= OnScoredGoal;
            gameManager.OnGameStarted -= OnGameStarted;
        }

        wall.SetActive(false);
        challengeModeScoreboard.SetActive(false);
        storyModeScoreboard.SetActive(true);
        ballManager.DestroyBall();
        timerCountdown.StopTimer();
        difficultyController.Stop();
        ReferencesHolder.Instance.CamerasController.SetMainMenuCamera();
        ReferencesHolder.Instance.ScreenFader.StartFadeIn
            (() => { ReferencesHolder.Instance.UIStateManager.OpenLayout(UILayoutsIDs.MainMenuLayout); });
    }

    public void RestartGameMode()
    {
        if (player1 != null)
        {
            Destroy(player1.gameObject);
            player1 = null;
        }

        ballManager.DestroyBall();
        gameManager.StartGameMode(GameModeEnums.GameModes.Challenge);
    }


    public string GetLevelDescription()
    {
        return levelDescription;
    }

    public void IncreaseDifficulty()
    {
        ballManager.ModifyBallSpeed(difficultyIncrease);
    }

    public void StartTimer()
    {
        timerCountdown.StartTimer(scoreBoard.SetTimerDisplay);
    }

    private void OnScoredGoal(int player)
    {
        ballManager.GoalScored();
        GameModeOver();
    }

    private void OnGameStarted()
    {
        StartTimer();
        ballManager.LaunchBall(1);
    }
  
}
