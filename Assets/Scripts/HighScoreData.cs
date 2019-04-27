using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HighScoreData
{
    public string playerName = "";
    public int scoreInSeconds = 0;

    public HighScoreData(string name, int score)
    {
        playerName = name;
        scoreInSeconds = score;
    }
    public HighScoreData()
    {
        playerName = "";
        scoreInSeconds = 0;
    }
}
