using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //debug
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<GameManager>();

            return instance;
        }
        set { instance = value; }
    }

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
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
  

        goalPlayer1.OnScoredGoal += OnScoreGoal;
        goalPlayer2.OnScoredGoal += OnScoreGoal;
        scoreManager.Initialize();
        timerCountdown.OnCountdownFinished += OnCountdownFinished;
        
        StartGame();
    }

    public void StartGame()
    {
        if (ball == null)
        {
            ball = Instantiate(ballPrefab);
            ball.name = "ball";
            ball.Initialize(ballStartPosition);
        }

        timerCountdown.StartCountdown(20);
        GameObject HumanPlayer = Instantiate(HumanPlayerPrefab);
        Instantiate(AIPlayerPrefab);
        skillHudController.Initialize(HumanPlayer);
    }

    private void OnScoreGoal(int player)
    {
        scoreManager.UpdateScoreGoal(player, 1);
        ball.GoalScored();
        ball.RespawnBall();

    }

    private void OnCountdownFinished()
    {
        ball.DisableBall();
    }
}
