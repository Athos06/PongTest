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

    [SerializeField]
    private Color highlightColor;
    [SerializeField]
    private RectTransform highscoresContainer;
    [SerializeField]
    private HighscoreEntry highScoreEntryPrefab;

    private void Awake()
    {
        ReferencesHolder.Instance.UIStateManager.OnLayoutOpen += OnLayoutOpen;
        Initialize();
    }

    private void Initialize()
    {
        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(OnContinueButtonClicked);
    }

    private void OnContinueButtonClicked()
    {
        ReferencesHolder.Instance.LeadersBoardManager.AddNewHighScore
            (inputField.text, ((ChallengeModeManager)ReferencesHolder.Instance.GameManager.GameMode).GetChallengeScore());

        ReferencesHolder.Instance.UIStateManager.CloseLastState();
    }

    private void Populate()
    {
        foreach (Transform child in highscoresContainer)
        {
            GameObject.Destroy(child.gameObject);
        }

        HighscoreEntry entry;
        var leadersBoardList = ReferencesHolder.Instance.LeadersBoardManager.LeaderBoardList;
        int score = ((ChallengeModeManager)ReferencesHolder.Instance.GameManager.GameMode).GetChallengeScore() ;
        int recordIndex = ReferencesHolder.Instance.LeadersBoardManager.CheckNewHighScore(score);

        int startIndex = recordIndex;
        if (recordIndex > 0)
        {
            //startIndex = (recordIndex < leadersBoardList.Count-1) ? startIndex = recordIndex - 1 : startIndex = startIndex - 1;
            startIndex = startIndex - 1;
        }

        bool added = false;
        for (int i = 0; i < 3; i++)
        {
            entry = Instantiate(highScoreEntryPrefab, highscoresContainer);

            if (startIndex == recordIndex && !added)
            {
                entry.Initialize((startIndex + 1).ToString("00"), "YOU", TimeFormatHelper.GetTimeInFormat(score));
                entry.PositionText.color = highlightColor;
                entry.NameText.color = highlightColor;
                entry.ScoreText.color = highlightColor;
                added = true;
            }
            else
            {
                entry.Initialize((startIndex + 1).ToString("00"), leadersBoardList[startIndex].playerName,
                TimeFormatHelper.GetTimeInFormat(leadersBoardList[startIndex].scoreInSeconds));
                startIndex++;
            }

       
        }

    }

    private void OnLayoutOpen(UILayoutsIDs id)
    {
        if (id == UILayoutsIDs.NewHighScoreLayout)
        {
            Populate();
        }
    }
}
