using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UIControl;

public class ChallengeStartMenuPanelController : MonoBehaviour
{
    [SerializeField]
    private Button startChallengeButton;
    [SerializeField]
    private Button backButton;
    [Space, SerializeField]
    private RectTransform leadersBoardContainer;
    [SerializeField]
    private HighscoreEntry highScoreEntryPrefab;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        ReferencesHolder.Instance.UIStateManager.OnLayoutOpen += OnLayoutOpen;
        startChallengeButton.onClick.RemoveAllListeners();
        startChallengeButton.onClick.AddListener(OnStartChallengeClicked);
        backButton.onClick.RemoveAllListeners();
        backButton.onClick.AddListener(OnBackButtonClicked);
    }

    private void OnStartChallengeClicked()
    {
        ReferencesHolder.Instance.GameManager.StartGameMode(GameModeEnums.GameModes.Challenge);
        ReferencesHolder.Instance.UIStateManager.CloseAll();
    }

    private void OnBackButtonClicked()
    {
        ReferencesHolder.Instance.UIStateManager.CloseLastState();
    }

    private void OnLayoutOpen(UILayoutsIDs id)
    {
        if (id == UILayoutsIDs.StartChallengeLayout)
        {
            Populate();
        }
    }

    private void Populate()
    {
        foreach (Transform child in leadersBoardContainer)
        {
            GameObject.Destroy(child.gameObject);
        }

        HighscoreEntry entry;
        int index = 1;
        foreach (var highscore in ReferencesHolder.Instance.LeadersBoardManager.LeaderBoardList)
        {
            entry = Instantiate(highScoreEntryPrefab, leadersBoardContainer);
            entry.Initialize(index.ToString("00"), highscore.playerName, TimeFormatHelper.GetTimeInFormat(highscore.scoreInSeconds));
            index++;
        }

    }

}
