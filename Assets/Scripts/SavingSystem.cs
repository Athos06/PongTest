using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public static class SavingSystem
{
    public static void SaveGame(int currentLevel)
    {
        string path = Application.persistentDataPath + "/saveFile.sf";
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);
        binaryFormatter.Serialize(stream, currentLevel);
        stream.Close();
    }

    public static int LoadGame()
    {
        string path = Application.persistentDataPath + "/saveFile.sf";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(path, FileMode.Open);

            int currentLevel = (int)formatter.Deserialize(fileStream);
            fileStream.Close();

            return currentLevel;
        }
        else
        {
            return -1;
        }
    }
}
