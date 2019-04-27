using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SavingManager
{
    public Action OnStoryModeStarted;

    private int continueLevel = -1;
    public int ContinueLevel { get { return continueLevel; } }
    private int numberOfLevels = 3;
    public int NumberOfLevels { get { return numberOfLevels; } }

    public void Initialize()
    {
        LoadGame();
    }

    public void UnlockNextLevel()
    {
        continueLevel++;
        if (continueLevel < numberOfLevels) { 
            SaveGame();
        }
        else
        {
            continueLevel = numberOfLevels - 1;
        }
    }

    public void RestartStoryMode()
    {
        continueLevel = 0;
        SaveGame();

        if (OnStoryModeStarted != null)
            OnStoryModeStarted.Invoke();
    }

    public void SaveGame()
    {
        SavingSystem.SaveGame(continueLevel);
    }

    public void LoadGame()
    {
        continueLevel = SavingSystem.LoadGame();
    }
    
}
