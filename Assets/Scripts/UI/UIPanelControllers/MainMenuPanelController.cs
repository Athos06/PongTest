using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UIControl;

public class MainMenuPanelController : MonoBehaviour
{
    [SerializeField]
    private Button startStoryModeButton;
    [SerializeField]
    private Button continueStoryModeButton;
    [SerializeField]
    private Button startChallengeModeButton;
    [SerializeField]
    private Button exitGameButton;

    // Start is called before the first frame update
    void Start()
    {
        ReferencesHolder.Instance.GameManager.SavingManager.OnStoryModeStarted += OnStoryModeStarted;
        startStoryModeButton.onClick.RemoveAllListeners();
        startStoryModeButton.onClick.AddListener(OnStartStoryModeClicked);
        startChallengeModeButton.onClick.RemoveAllListeners();
        startChallengeModeButton.onClick.AddListener(OnStartChallengeModeClicked);
        continueStoryModeButton.onClick.RemoveAllListeners();
        continueStoryModeButton.onClick.AddListener(OnContinueStoryModeClicked);
        exitGameButton.onClick.RemoveAllListeners();
        exitGameButton.onClick.AddListener(OnExitGameClicked);

        if (ReferencesHolder.Instance.GameManager.SavingManager.ContinueLevel == -1)
        {
            continueStoryModeButton.interactable = false;
        }
    }

    private void OnStartStoryModeClicked()
    {
        ReferencesHolder.Instance.GameManager.SavingManager.RestartStoryMode();
        ReferencesHolder.Instance.GameManager.StartGameMode(GameManager.GameModes.Story);
        ReferencesHolder.Instance.UIStateManager.CloseAll();
    }

    private void OnContinueStoryModeClicked()
    {
        ReferencesHolder.Instance.GameManager.StartGameMode(GameManager.GameModes.Story);
        ReferencesHolder.Instance.UIStateManager.CloseAll();
    }

    private void OnStartChallengeModeClicked()
    {
        ReferencesHolder.Instance.UIStateManager.OpenLayout(UILayoutsIDs.StartChallengeLayout);
    }

    private void OnExitGameClicked()
    {
        Application.Quit();
    }


    private void OnStoryModeStarted()
    {
        continueStoryModeButton.interactable = true;
    }
}
