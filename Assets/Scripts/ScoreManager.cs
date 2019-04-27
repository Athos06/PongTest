using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    int[] playersScore = { 0, 0 };
    public int[] PlayersScore {  get { return playersScore; } }
    [SerializeField]
    TextMeshProUGUI player1ScoreText;
    [SerializeField]
    TextMeshProUGUI player2ScoreText;

    public void Initialize()
    {
        player1ScoreText.text = 0.ToString("00");
        player2ScoreText.text = 0.ToString("00");
        for(int i = 0; i< playersScore.Length; i++ ) playersScore[i] = 0;
    }

    public void ResetScore()
    {
        player1ScoreText.text = 0.ToString("00");
        player2ScoreText.text = 0.ToString("00");
        for (int i = 0; i < playersScore.Length; i++) playersScore[i] = 0;
    }

    public void UpdateScoreGoal(int player, int score)
    {
        playersScore[player] += score;
        if(player == 0)
            player1ScoreText.text = playersScore[player].ToString("00");
        else
            player2ScoreText.text = playersScore[player].ToString("00");
    }


}
