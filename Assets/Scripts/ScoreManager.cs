using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    int[] playerScore = { 0, 0 };

    [SerializeField]
    TextMeshProUGUI player1ScoreText;
    [SerializeField]
    TextMeshProUGUI player2ScoreText;

    public void Initialize()
    {
        player1ScoreText.text = 0.ToString();
        player2ScoreText.text = 0.ToString();
    }

    public void UpdateScoreGoal(int player, int score)
    {
        playerScore[player] += score;
        Debug.Log("Player  " + player);
        if(player == 0)
            player1ScoreText.text = playerScore[player].ToString();
        else
            player2ScoreText.text = playerScore[player].ToString();
    }


}
