using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public static class SaveDataManager
{
    private static SaveData saveData = new SaveData();

    public static void LoadGame()
    {
        string path = SavePath();
        if (File.Exists(path))
        {
            string fileContents = File.ReadAllText(path);

            saveData = JsonUtility.FromJson<SaveData>(fileContents);
        }
    }

    public static void SaveGame()
    {
        string path = SavePath();
        string jsonString = JsonUtility.ToJson(saveData);

        File.WriteAllText(path, jsonString);
    }

    static string SavePath()
    {
        return Application.persistentDataPath + "/playerProgress.json";
    }

    public static void SaveCompleteLevel(string level)
    {
        var completion = new LevelCompletionData(level);

        saveData.levelCompletions[completion.key] = completion;

        SaveGame();
    }

    public static bool IsLevelComplete(string level)
    {
        return saveData.levelCompletions.ContainsKey(level);  
    }
}
