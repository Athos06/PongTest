using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIControl;
using UnityEngine.UI;

public class StoryModeFinishedPanelController : MonoBehaviour
{
    [SerializeField]
    private Button continueButton;


    private void Start()
    {
        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(OnContinueButtonClicked);
    }

    private void OnContinueButtonClicked()
    {
        ReferencesHolder.Instance.UIStateManager.CloseAll();
        ReferencesHolder.Instance.GameManager.FinishGame();
    }

}
