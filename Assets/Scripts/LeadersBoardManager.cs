using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeadersBoardManager : MonoBehaviour
{

    [SerializeField]
    private List<HighScoreData> leaderBoardList = new List<HighScoreData>();
    public List<HighScoreData> LeaderBoardList {  get { return leaderBoardList; } }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CheckNewHighScore(int score)
    {
        foreach(var highScore in leaderBoardList)
        {
            if(score > highScore.scoreInSeconds)
            {
                return true;
            }
        }

        return false;
    }

    public void AddNewHighScore(string name, int score)
    {
        Debug.Log("trying to add new Highscore " + name + " " + score);
        HighScoreData newHighScore = new HighScoreData(name, score);
        HighScoreData replaceHighScore = new HighScoreData();

        bool replaced = false;
        for(int i = 0; i < leaderBoardList.Count; i++)
        {
            if (replaced)
            {
                HighScoreData tempHighScore = leaderBoardList[i];
                leaderBoardList[i] = replaceHighScore;
            }
            else if (score > leaderBoardList[i].scoreInSeconds)
            {
                replaceHighScore = leaderBoardList[i];
                leaderBoardList[i] = newHighScore;
                replaced = true;
            }
        }

        SaveLeadersBoard();
    }



    public void Initialize()
    {
        LoadLeadersBoard();
    }

    [ContextMenu("CreateFile")]
    public void SaveLeadersBoard()
    {
        LeaderboardSystem.SaveHighscore(leaderBoardList);
    }

    [ContextMenu("LoadFile")]
    public void LoadLeadersBoard()
    {
        leaderBoardList = LeaderboardSystem.LoadHighScore();

        ////debug
        //foreach (var hs in LeaderboardSystem.LoadHighScore())
        //{
        //    Debug.Log(hs.playerName);
        //}
    }

}
