using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UIControl;
using TMPro;

public class StoryLevelFinishedPanelController : MonoBehaviour
{
    [SerializeField]
    private Button continueButton;
    [SerializeField]
    private Button restartButton;
    [SerializeField]
    private Button exitToMainMenuButton;

    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private TextMeshProUGUI winnerText;
    [SerializeField]
    private Color winnerTextColor;
    [SerializeField]
    private Color loserTextColor;
    [SerializeField]
    private Color drawTextColor;

    private int currentLevel;
    private int nextLevel;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        ReferencesHolder.Instance.UIStateManager.OnLayoutOpen += OnLayoutOpen;
        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(OnContinueButtonClicked);
        exitToMainMenuButton.onClick.RemoveAllListeners();
        exitToMainMenuButton.onClick.AddListener(OnExitToMainMenuClicled);
        restartButton.onClick.RemoveAllListeners();
        restartButton.onClick.AddListener(OnRestartButtonClicked);
    }

    private void OnContinueButtonClicked()
    {
        //Debug.Log("level " + ReferencesHolder.Instance.GameManager.SavingManager.ContinueLevel + " number of levels " + ReferencesHolder.Instance.GameManager.SavingManager.NumberOfLevels);
        if(currentLevel < (ReferencesHolder.Instance.GameManager.SavingManager.NumberOfLevels-1))
        {
            ReferencesHolder.Instance.UIStateManager.CloseAll();
            ReferencesHolder.Instance.GameManager.RestartGame();
        }
        else
        {
            ReferencesHolder.Instance.UIStateManager.CloseAll();
            ReferencesHolder.Instance.UIStateManager.OpenLayout(UILayoutsIDs.StoryModeFinishedLayout);

        }
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
        //ReferencesHolder.Instance.UIStateManager.OpenLayout(UILayoutsIDs.MainMenuLayout);
    }

    private void OnLayoutOpen(UILayoutsIDs id)
    {
        if (id == UILayoutsIDs.StoryLevelFinishedLayout)
        {
            Populate();
        }
    }

    private void Populate()
    {
        int[] score = ReferencesHolder.Instance.GameManager.GetScore();
        scoreText.text = score[0].ToString() + " - " + score[1].ToString();
        continueButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(true);

        if (ReferencesHolder.Instance.GameManager.GetWinnerPlayer() == 0)
        {
            currentLevel = ReferencesHolder.Instance.GameManager.SavingManager.ContinueLevel;
            ReferencesHolder.Instance.GameManager.SavingManager.UnlockNextLevel();
            continueButton.gameObject.SetActive(true);
            restartButton.gameObject.SetActive(false);
            winnerText.text = "YOU WON";
            winnerText.color = winnerTextColor;
        }
        if (ReferencesHolder.Instance.GameManager.GetWinnerPlayer() == 1)
        {
            winnerText.text = "YOU LOST";
            winnerText.color = loserTextColor;
        }
        if (ReferencesHolder.Instance.GameManager.GetWinnerPlayer() == -1)
        {
            winnerText.text = "DRAW";
            winnerText.color = drawTextColor;
        }
    }
}
