using UnityEngine;
using UIControl;
using System;

public class GameManager : MonoBehaviour
{
    #region Events
    public Action OnGameStarted;
    public Action OnGameOver;
    public Action<bool> OnGamePause;
    #endregion
    public bool IsGamePaused { get; private set; }
    public bool IsGamePlaying { get; private set; }
    
    [SerializeField]
    private SkillHudController skillHudController;
    public SkillHudController SKillHudController { get { return skillHudController; } }

    [SerializeField]
    private StoryModeManager storyModeManager;
    [SerializeField]
    private ChallengeModeManager challengeModeManager;
    [SerializeField]
    private StartGameCountdownController startGameCountdown;

    private IGameMode gameMode;
    public IGameMode GameMode { get { return gameMode; } }
    private SavingManager savingManager;
    public SavingManager SavingManager { get { return savingManager; } }
    private Coroutine StartingGameCoroutine;

    public void Initialize()
    {
        savingManager = new SavingManager();
        savingManager.Initialize();
        storyModeManager.Initialize(this);
        challengeModeManager.Initialize(this);
        ReferencesHolder.Instance.UIStateManager.OpenLayout(UILayoutsIDs.MainMenuLayout);
    }

    public void PauseGame()
    {
        if (IsGamePlaying)
        {
            Time.timeScale = 0;
            IsGamePaused = true;
            ReferencesHolder.Instance.UIStateManager.OpenLayout(UILayoutsIDs.PauseLayout, true);
            if (OnGamePause != null)
                OnGamePause.Invoke(true);
        }
    }

    public void UnPauseGame()
    {
        if (IsGamePlaying)
        {
            Time.timeScale = 1;
            IsGamePaused = false;
            ReferencesHolder.Instance.UIStateManager.CloseLastState();
            if (OnGamePause != null)
                OnGamePause.Invoke(false);
        }
    }

    public void QuitGame()
    {
        if (StartingGameCoroutine != null)
        {
            StopCoroutine(StartingGameCoroutine);
            StartingGameCoroutine = null;
        }

        ReferencesHolder.Instance.UIStateManager.ClosePanel(UIPanelsIDs.StartCountDownPanel);
        ReferencesHolder.Instance.ScreenFader.StartFadeOut( () => 
        {
            UnPauseGame();
            IsGamePlaying = false;
            gameMode.FinishGameMode();
            GameOver();
        });
    }

    public void StartGameMode(GameModeEnums.GameModes gameMode)
    {
        switch (gameMode)
        {
            case GameModeEnums.GameModes.Story:
                this.gameMode = storyModeManager;
                break;
            case GameModeEnums.GameModes.Challenge:
                this.gameMode = challengeModeManager;
                break;
        }

        ReferencesHolder.Instance.ScreenFader.StartFadeOut(this.gameMode.StartGameMode);
    }

    public void StartGame()
    {
        IsGamePlaying = true;
        if (OnGameStarted != null)
            OnGameStarted.Invoke();
    }

    public void GameOver()
    {
        if (OnGameOver != null)
            OnGameOver.Invoke();
    }

    public void FinishGame()
    {
        IsGamePlaying = false;
        ReferencesHolder.Instance.ScreenFader.StartFadeOut(gameMode.FinishGameMode);
    }

    public void RestartGame()
    {
        ReferencesHolder.Instance.ScreenFader.StartFadeOut(gameMode.RestartGameMode);
    }

}
