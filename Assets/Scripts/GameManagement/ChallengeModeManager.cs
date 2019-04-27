using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIControl;
using TMPro;

public class ChallengeModeManager : MonoBehaviour, IGameMode
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
    private float introLength = 3.0f;
    [SerializeField]
    private CharacterController HumanPlayerPrefab;

    [SerializeField]
    private TextMeshProUGUI timerText;


    private CharacterController player1;

    private bool subcribedToEvents = false;

    public void Initialize(GameManager gameManager)
    {
        this.gameManager = gameManager;
        
    }

    public void StartGameMode()
    {
        if(!subcribedToEvents)
        {
            subcribedToEvents = true;
            gameManager.OnScoredGoalEvent += OnScoredGoal;
            gameManager.OnGameStarted += OnGameStarted;
        }

        timerText.text = "0:00";
        wall.SetActive(true);
        challengeModeScoreboard.SetActive(true);
        storyModeScoreboard.SetActive(false);

        player1 = Instantiate(HumanPlayerPrefab);

        ReferencesHolder.Instance.CamerasController.SetEnemyIntroCamera();
        ReferencesHolder.Instance.ScreenFader.StartFadeIn(StartIntro);

    }

    private void StartIntro()
    {
        StartCoroutine(PlayIntro());
    }

    private IEnumerator PlayIntro()
    {
        ReferencesHolder.Instance.UIStateManager.OpenPanel(UIPanelsIDs.EnemyNamePanel);
        yield return new WaitForSeconds(introLength);
        ReferencesHolder.Instance.UIStateManager.ClosePanel(UIPanelsIDs.EnemyNamePanel);
        gameManager.StartGame();
        yield return null;
    }

    public void GameModeOver()
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

    public void FinishGameMode()
    {
        if(player1 != null)
        {
            Destroy(player1.gameObject);
            player1 = null;
        }

        if (subcribedToEvents)
        {
            subcribedToEvents = false;
            gameManager.OnScoredGoalEvent -= OnScoredGoal;
            gameManager.OnGameStarted -= OnGameStarted;
        }

        wall.SetActive(false);
        challengeModeScoreboard.SetActive(false);
        storyModeScoreboard.SetActive(true);

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

        gameManager.StartGameMode(GameManager.GameModes.Challenge);

    }

    private void OnScoredGoal(int player)
    {
        GameModeOver();
    }

    private void OnGameStarted()
    {
        gameManager.StartTimer(timerText, false);
    }
}
