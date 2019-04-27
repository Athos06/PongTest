using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UIControl;

public class StoryModeManager : MonoBehaviour
{

    GameManager gameManager;

    //debug
    public CharacterController enemy1Prefab;
    public CharacterController enemy2Prefab;
    public CharacterController enemy3Prefab;

    [SerializeField]
    private TextMeshProUGUI timerText;

    public int level = 0;
    public int GetLevel()
    {
        return level;
    }

    public void Initialize(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }
    
    public void StartNextLevel()
    {
        StartLevel(GetLevel());
    }

    public void StartLevel(int level)
    {
        gameManager.OnCountdownFinishedEvent += OnCountdownFinishedEvent;
        gameManager.StartGame();
        gameManager.StartTimer(timerText, true);
        CharacterController AIPlayer;
        switch (level)
        {
            case 0:
                AIPlayer = Instantiate(enemy1Prefab);
                break;
            case 1:
                AIPlayer = Instantiate(enemy2Prefab);
                break;
            case 2:
                AIPlayer = Instantiate(enemy3Prefab);
                break;
        }
    }

    private void OnCountdownFinishedEvent()
    {
        gameManager.GameOver();
        ReferencesHolder.Instance.UIStateManager.CloseAll();
        ReferencesHolder.Instance.UIStateManager.OpenLayout(UILayoutsIDs.StoryLevelFinishedLayout);
    }
}
