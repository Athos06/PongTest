using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighscoreEntry : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI positionText;
    public TextMeshProUGUI PositionText { get { return positionText; } }
    [SerializeField]
    private TextMeshProUGUI nameText;
    public TextMeshProUGUI NameText { get { return nameText; } }
    [SerializeField]
    private TextMeshProUGUI scoreText;
    public TextMeshProUGUI ScoreText { get { return scoreText; } }

    public void Initialize(string position, string name, string score)
    {
        positionText.text = position;
        nameText.text = name;
        scoreText.text = score;
    }
    

}
