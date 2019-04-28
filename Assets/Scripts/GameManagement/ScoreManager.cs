using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ScoreManager
{
    private int[] playersScore = { 0, 0 };
    public int[] PlayersScore {  get { return playersScore; } }
    private StoryModeScoreBoard scoreBoard;

    public ScoreManager(StoryModeScoreBoard scoreBoard)
    {
        this.scoreBoard = scoreBoard;
        ResetScore();
    }

    public void ResetScore()
    {
        Debug.Log("ok i reset score");
        for (int i = 0; i < playersScore.Length; i++) playersScore[i] = 0;
        DisplayScore();
    }

    public void UpdateScoreGoal(int player, int score)
    {
        Debug.Log("Update score goal");
        playersScore[player] += score;
        DisplayScore();
    }

    private void DisplayScore()
    {
        scoreBoard.SetScoreDisplay(playersScore[0].ToString("00"), playersScore[1].ToString("00"));
    }

}
