using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeadersBoardManager : MonoBehaviour
{

    [SerializeField]
    private List<HighScoreData> leaderBoardList = new List<HighScoreData>();
    public List<HighScoreData> LeaderBoardList { get { return leaderBoardList; } }

    //// Start is called before the first frame update
    //void Start()
    //{
    //    Initialize();
    //}

    // Update is called once per frame
    void Update()
    {

    }

    public int CheckNewHighScore(int score)
    {
        int index = 0;
        foreach (var highScore in leaderBoardList)
        {
            if (score > highScore.scoreInSeconds)
            {
                return index;
            }
            index++;
        }

        return -1;
    }

    public void AddNewHighScore(string name, int score)
    {
        HighScoreData newHighScore = new HighScoreData(name, score);

        for (int i = 0; i < leaderBoardList.Count; i++)
        {
            if (score > leaderBoardList[i].scoreInSeconds)
            {
                var tempList = leaderBoardList.GetRange(i, leaderBoardList.Count - 1 - i);
                //Debug.Log("remove range i " + i + " count " + (leaderBoardList.Count - 1 - i) );
                leaderBoardList.RemoveRange(i, leaderBoardList.Count - i);
                leaderBoardList.Add(newHighScore);
                //foreach (var a in leaderBoardList) Debug.Log(a.playerName);
                leaderBoardList.AddRange(tempList);
                //Debug.Log("======================");
                //foreach (var a in leaderBoardList) Debug.Log(a.playerName);
                break;
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
        var list = LeaderboardSystem.LoadHighScore();
        if (list == null)
            SaveLeadersBoard();
    }

}
