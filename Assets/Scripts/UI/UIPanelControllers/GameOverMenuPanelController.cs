using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UIControl;

public class GameOverMenuPanelController : MonoBehaviour
{
    [SerializeField]
    private Button continueButton;
    [SerializeField]
    private Button exitToMainMenuButton;

    private void Start()
    {
        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(OnContinueButtonClicked);
        exitToMainMenuButton.onClick.RemoveAllListeners();
        exitToMainMenuButton.onClick.AddListener(OnExitToMainMenuClicled);
    }

    private void OnContinueButtonClicked()
    {
        ReferencesHolder.Instance.UIStateManager.CloseAll();
        ReferencesHolder.Instance.GameManager.StartGame();
    }

    private void OnExitToMainMenuClicled()
    {
        ReferencesHolder.Instance.UIStateManager.CloseAll();
        ReferencesHolder.Instance.UIStateManager.OpenLayout(UILayoutsIDs.MainMenuLayout);
    }
}
