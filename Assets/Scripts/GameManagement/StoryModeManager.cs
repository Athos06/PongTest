using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryModeManager : MonoBehaviour
{

    GameManager gameManager;

    //debug
    public CharacterController enemy1Prefab;
    public CharacterController enemy2Prefab;
    public CharacterController enemy3Prefab;

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
        gameManager.StartGame();
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

}
