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
        startStoryModeButton.onClick.RemoveAllListeners();
        startStoryModeButton.onClick.AddListener(OnStartStoryModeClicked);
        startChallengeModeButton.onClick.RemoveAllListeners();
        startChallengeModeButton.onClick.AddListener(OnStartChallengeModeClicked);
    }
    
    private void OnStartStoryModeClicked()
    {
        ReferencesHolder.Instance.GameManager.StartGameMode(GameManager.GameModes.Story);
        ReferencesHolder.Instance.UIStateManager.CloseAll();
        //ReferencesHolder.Instance.UIStateManager.OpenLayout(UILayoutsIDs.HUDLayout);
    }
    private void OnStartChallengeModeClicked()
    {
        ReferencesHolder.Instance.GameManager.StartGameMode(GameManager.GameModes.Challenge);
        ReferencesHolder.Instance.UIStateManager.CloseAll();
        //ReferencesHolder.Instance.UIStateManager.OpenLayout(UILayoutsIDs.HUDLayout);

    }
}
