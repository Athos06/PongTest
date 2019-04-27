using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UIControl;


public class ChallengeLevelFinishedPanelController : MonoBehaviour
{
    [SerializeField]
    private Button restartButton;
    [SerializeField]
    private Button exitToMainMenuButton;

    private void Start()
    {
        restartButton.onClick.RemoveAllListeners();
        restartButton.onClick.AddListener(OnContinueButtonClicked);
        exitToMainMenuButton.onClick.RemoveAllListeners();
        exitToMainMenuButton.onClick.AddListener(OnExitToMainMenuClicled);
    }

    private void OnContinueButtonClicked()
    {
        //debug
        ReferencesHolder.Instance.UIStateManager.CloseAll();
        ReferencesHolder.Instance.GameManager.RestartGame();
    }

    private void OnExitToMainMenuClicled()
    {
        ReferencesHolder.Instance.UIStateManager.CloseAll();
        ReferencesHolder.Instance.GameManager.FinishGame();

        //ReferencesHolder.Instance.UIStateManager.OpenLayout(UILayoutsIDs.MainMenuLayout);
    }
}
