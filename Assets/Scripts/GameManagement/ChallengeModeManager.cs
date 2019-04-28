﻿using System.Collections;
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
    [SerializeField]
    private GameObject wall;
    [SerializeField]
    private GameObject challengeModeScoreboard;
    [SerializeField]
    private GameObject storyModeScoreboard;
    [SerializeField]
    private float introLength = 3.0f;
    [SerializeField]
    private Vector3 ballStartPosition;
    [SerializeField]
    private CharacterController HumanPlayerPrefab;
    [SerializeField]
    private float difficultyIncrease = 0.25f;

    [SerializeField]
    private ChallengeModeScoreboard scoreBoard;
    [SerializeField]
    private ChallengeDifficultyController difficultyController;

    [TextArea, SerializeField]
    private string levelDescription;
    
    public string GetLevelDescription()
    {
        return levelDescription;
    }

    private CharacterController player1;

    private bool subcribedToEvents = false;

    

    public void Initialize(GameManager gameManager)
    {
        this.gameManager = gameManager;
        difficultyController.Initialize(this);
        
    }

    public void IncreaseDifficulty()
    {
        gameManager.BallManager.ModifyBallSpeed(difficultyIncrease);
    }


    public void StartGameMode()
    {
        if(!subcribedToEvents)
        {
            subcribedToEvents = true;
            gameManager.OnScoredGoalEvent += OnScoredGoal;
            gameManager.OnGameStarted += OnGameStarted;
        }

        

        scoreBoard.Initialize();
        wall.SetActive(true);
        challengeModeScoreboard.SetActive(true);
        storyModeScoreboard.SetActive(false);

        player1 = Instantiate(HumanPlayerPrefab);
        gameManager.SKillHudController.Initialize(player1.GetComponent<CharacterSkillController>());

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
        gameManager.BallManager.SpawnBall(ballStartPosition);
        gameManager.StartGame();
        difficultyController.StartDiffiultyIncreasing();
        yield return null;
    }

    public int score;

    public void GameModeOver()
    {
        difficultyController.Stop();
        score = gameManager.TimerCountdown.TimerCurrentTime;

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

        gameManager.StartGameMode(GameModeEnums.GameModes.Challenge);

    }

    private void OnScoredGoal(int player)
    {
        gameManager.BallManager.GoalScored();
        GameModeOver();

    }

    private void OnGameStarted()
    {
        gameManager.StartTimer(scoreBoard.TimerText, false);
        gameManager.BallManager.LaunchBall(1);
    }
  
}
