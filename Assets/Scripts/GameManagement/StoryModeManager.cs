using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UIControl;

public class StoryModeManager : MonoBehaviour, IGameMode
{
    [SerializeField]
    public CharacterController enemy1Prefab;
    [SerializeField]
    public CharacterController enemy2Prefab;
    [SerializeField]
    public CharacterController enemy3Prefab;
    [SerializeField]
    private CharacterController HumanPlayerPrefab;

    [Header("References")]
    [SerializeField]
    private Vector3 ballStartPosition;
    [TextArea, SerializeField]
    private string[] levelsDescription;
    [SerializeField]
    private StoryModeScoreBoard storyModeScoreBoard;
    [SerializeField]
    private TimerCountdown timerCountdown;
    public TimerCountdown TimerCountdown { get { return timerCountdown; } }
    [SerializeField]
    private StartGameCountdownController startGameCountdown;
    [SerializeField]
    private GoalsManager goalsManager;
    [Header("game values")]
    [SerializeField]
    private int RoundTime = 60;
    [SerializeField]
    private float countdown = 3.0f;
    [SerializeField]
    private float introLength = 3.0f;

    private CharacterController player1;
    private CharacterController player2;
    private GameManager gameManager;
    private ScoreManager scoreManager;
    private bool subcribedToEvents = false;

    public void Initialize(GameManager gameManager)
    {
        this.gameManager = gameManager;
        scoreManager = new ScoreManager(storyModeScoreBoard);
        goalsManager.Initialize();
    }

    public string GetLevelDescription()
    {
        return levelsDescription[GetLevel()];
    }

    public int GetLevel()
    {
        return ReferencesHolder.Instance.GameManager.SavingManager.ContinueLevel;
    }

    public void StartLevel(int level)
    {
        ReferencesHolder.Instance.CamerasController.SetEnemyIntroCamera();


        if (!subcribedToEvents)
        {
            subcribedToEvents = true;
            gameManager.OnGameStarted += OnGameStarted;
            timerCountdown.OnCountdownFinished += OnCountdownFinishedEvent;
            goalsManager.OnScoredGoal += OnScoredGoal;
        }

        switch (level)
        {
            case 0:
                player2 = Instantiate(enemy1Prefab);
                break;
            case 1:
                player2 = Instantiate(enemy2Prefab);
                break;
            case 2:
                player2 = Instantiate(enemy3Prefab);
                break;
        }

        player1 = Instantiate(HumanPlayerPrefab);
        gameManager.SKillHudController.Initialize(player1.GetComponent<CharacterSkillController>());

        ReferencesHolder.Instance.ScreenFader.StartFadeIn(StartEnemyIntro);
    }

    public void StartGameMode()
    {
        scoreManager.ResetScore();

        int nextLevel = GetLevel();
        if (nextLevel == -1)
        {
            nextLevel = 0;
            ReferencesHolder.Instance.GameManager.SavingManager.RestartStoryMode();
        }

        StartLevel(nextLevel);
    }

    public void GameModeOver()
    {
        timerCountdown.StopTimer();
        Debug.Log("reset the score");
        gameManager.BallManager.DisableBall();
        gameManager.GameOver();
        ReferencesHolder.Instance.UIStateManager.CloseAll();
        ReferencesHolder.Instance.UIStateManager.OpenLayout(UILayoutsIDs.StoryLevelFinishedLayout);
    }

    public void FinishGameMode()
    {
        if (player1 != null)
        {
            Destroy(player1.gameObject);
            player1 = null;
        }
        if (player2 != null)
        {
            Destroy(player2.gameObject);
            player2 = null;
        }

        if (subcribedToEvents)
        {
            subcribedToEvents = false;
            timerCountdown.OnCountdownFinished -= OnCountdownFinishedEvent;
            gameManager.OnGameStarted -= OnGameStarted;
            goalsManager.OnScoredGoal -= OnScoredGoal;
        }

        scoreManager.ResetScore();

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
        if (player2 != null)
        {
            Destroy(player2.gameObject);
            player2 = null;
        }

        scoreManager.ResetScore();
        gameManager.StartGameMode(GameModeEnums.GameModes.Story);
    }

    private void StartEnemyIntro()
    {
        StartCoroutine(PlayEnemyIntro());
    }

    private IEnumerator PlayEnemyIntro()
    {
        ReferencesHolder.Instance.UIStateManager.OpenPanel(UIPanelsIDs.EnemyNamePanel);
        yield return new WaitForSeconds(introLength);
        ReferencesHolder.Instance.UIStateManager.ClosePanel(UIPanelsIDs.EnemyNamePanel);
        gameManager.BallManager.SpawnBall(ballStartPosition);

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
        yield return null;
    }

    private void OnCountdownFinishedEvent()
    {
        GameModeOver();
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

    public void StartTimer()
    {
        timerCountdown.StartCountdown(storyModeScoreBoard.SetTimerDisplay, RoundTime);


        //else
        //{
        //    timerCountdown.StartTimer(timerText);
        //}
    }

    private void OnGameStarted()
    {
        StartTimer();
        gameManager.BallManager.LaunchBall();
    }

    private void OnScoredGoal(int player)
    {
        scoreManager.UpdateScoreGoal(player, 1);
        gameManager.BallManager.GoalScored();
        gameManager.BallManager.KickOff(ballStartPosition, player);
    }

}
