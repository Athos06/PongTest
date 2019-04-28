using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UIControl;
using TMPro;

public class ChallengeLevelFinishedPanelController : MonoBehaviour
{
    [SerializeField]
    private Button restartButton;
    [SerializeField]
    private Button exitToMainMenuButton;

    [SerializeField]
    private TextMeshProUGUI scoreText;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        ReferencesHolder.Instance.UIStateManager.OnLayoutOpen += OnLayoutOpen;
        restartButton.onClick.RemoveAllListeners();
        restartButton.onClick.AddListener(OnRestartButtonClicked);
        exitToMainMenuButton.onClick.RemoveAllListeners();
        exitToMainMenuButton.onClick.AddListener(OnExitToMainMenuClicled);
    }

    private void OnRestartButtonClicked()
    {
        ReferencesHolder.Instance.UIStateManager.CloseAll();
        ReferencesHolder.Instance.GameManager.RestartGame();
    }

    private void OnExitToMainMenuClicled()
    {
        ReferencesHolder.Instance.UIStateManager.CloseAll();
        ReferencesHolder.Instance.GameManager.FinishGame();
    }

    private void Populate()
    {
        scoreText.text = TimeFormatHelper.GetTimeInFormat(ReferencesHolder.Instance.GameManager.GetChallengeScore());
    }

    private void OnLayoutOpen(UILayoutsIDs id)
    {
        if(id == UILayoutsIDs.ChallengeLevelFinishedLayout)
        {
            Populate();
        }
    }
}
