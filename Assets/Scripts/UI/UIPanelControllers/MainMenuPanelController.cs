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
    }
    
    private void OnStartStoryModeClicked()
    {
        ReferencesHolder.Instance.GameManager.StartStoryMode();
        ReferencesHolder.Instance.UIStateManager.CloseAll();
        ReferencesHolder.Instance.UIStateManager.OpenLayout(UILayoutsIDs.HUDLayout);

    }
}
