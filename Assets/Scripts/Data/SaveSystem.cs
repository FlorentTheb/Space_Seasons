using UnityEngine;
using System.IO;
using System.Collections.Generic;

public static class SaveSystem
{
    private static readonly string savePath = Path.Combine(Application.persistentDataPath, "savefile.json");

    public static void SaveBestScores(List<int> bestScores)
    {
        SaveData data = new()
        {
            bestScores = bestScores
        };

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(savePath, json);
    }

    public static List<int> LoadBestScores()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            return data.bestScores;
        }

        return new List<int>();
    }
}
