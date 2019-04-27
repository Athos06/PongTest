using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public static class LeaderboardSystem
{

    public static void SaveHighscore(List<HighScoreData> leaderBoard)
    {
        string path = Application.persistentDataPath + "leadersBoard.lb";
        StreamWriter streamWritter = new StreamWriter(path);

        foreach (var highScore in leaderBoard)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(memoryStream, highScore);
            string str = Convert.ToBase64String(memoryStream.ToArray());
            streamWritter.WriteLine(str);
        }
        streamWritter.Close();

    }

    public static List<HighScoreData> LoadHighScore()
    {
        List<HighScoreData> leadersBoard = new List<HighScoreData>();

        string path = Application.persistentDataPath + "leadersBoard.lb";
        StreamReader streamReader = new StreamReader(path);

        while (!streamReader.EndOfStream)
        {
            //Read one line at a time
            string objectStream = streamReader.ReadLine();
            //Convert the Base64 string into byte array
            byte[] memorydata = Convert.FromBase64String(objectStream);
            MemoryStream memoryStream = new MemoryStream(memorydata);
            BinaryFormatter formatter = new BinaryFormatter();
            //Create object using BinaryFormatter
            HighScoreData highScore = (HighScoreData)formatter.Deserialize(memoryStream);

            leadersBoard.Add(highScore);

        }

        return leadersBoard;
    }
}
