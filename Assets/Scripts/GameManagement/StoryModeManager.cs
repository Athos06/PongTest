using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UIControl;

public class StoryModeManager : MonoBehaviour, IGameMode
{

    GameManager gameManager;

    //debug
    public CharacterController enemy1Prefab;
    public CharacterController enemy2Prefab;
    public CharacterController enemy3Prefab;

    [SerializeField]
    private CharacterController HumanPlayerPrefab;

    [SerializeField]
    private float introLength = 3.0f;

    [SerializeField]
    private TextMeshProUGUI timerText;
    [SerializeField]
    private Vector3 ballStartPosition;

    [TextArea, SerializeField]
    private string[] levelsDescription;

    public string GetLevelDescription()
    {
        return levelsDescription[GetLevel()];
    }

    private CharacterController player1;
    private CharacterController player2;

    private bool subcribedToEvents = false;

    public int GetLevel()
    {
        return ReferencesHolder.Instance.GameManager.SavingManager.ContinueLevel;
    }

    public void Initialize(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public void StartLevel(int level)
    {
        ReferencesHolder.Instance.CamerasController.SetEnemyIntroCamera();


        if (!subcribedToEvents)
        {
            subcribedToEvents = true;
            gameManager.OnCountdownFinishedEvent += OnCountdownFinishedEvent;
            gameManager.OnGameStarted += OnGameStarted;
            gameManager.OnScoredGoalEvent += OnGoalScore;
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
        gameManager.StartGame();
        yield return null;
    }

    private void OnCountdownFinishedEvent()
    {
        GameModeOver();
    }

    public void StartGameMode()
    {
        

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
        gameManager.GameOver();
        ReferencesHolder.Instance.UIStateManager.CloseAll();
        ReferencesHolder.Instance.UIStateManager.OpenLayout(UILayoutsIDs.StoryLevelFinishedLayout);
    }

    public void FinishGameMode()
    {
        if(player1 != null)
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
            gameManager.OnCountdownFinishedEvent -= OnCountdownFinishedEvent;
            gameManager.OnGameStarted -= OnGameStarted;
        }

        ReferencesHolder.Instance.ScreenFader.StartFadeIn
            (() => { ReferencesHolder.Instance.UIStateManager.OpenLayout(UILayoutsIDs.MainMenuLayout); } );
        
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

        gameManager.StartGameMode(GameManager.GameModes.Story);
    }

    private void OnGameStarted()
    {
        gameManager.StartTimer(timerText, true);
        gameManager.BallManager.KickOff(ballStartPosition);
    }

    private void OnGoalScore(int player)
    {
        gameManager.BallManager.GoalScored();
        gameManager.BallManager.KickOff(ballStartPosition);
    }
}
