using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UIControl;
using TMPro;

public class NewHighScorePanelController : MonoBehaviour
{
    [SerializeField]
    private Button continueButton;
    [SerializeField]
    private TMP_InputField inputField;

    private void Start()
    {
        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(OnContinueButtonClicked);
    }

    private void OnContinueButtonClicked()
    {
        Debug.Log("on continue button clicked");

        ReferencesHolder.Instance.LeadersBoardManager.AddNewHighScore
            (inputField.text, ReferencesHolder.Instance.GameManager.GetChallengeScore());

        ReferencesHolder.Instance.UIStateManager.CloseLastState();
    }
}
