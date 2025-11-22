using System;
using System.IO;
using UnityEngine;

public class SaveSystem
{
    private static string SavePath(int slot) =>
        Path.Combine(Application.persistentDataPath, $"save_{slot}.json");

    public static void SaveGame(SaveData data, int slot)
    {
        string json = JsonUtility.ToJson(data, prettyPrint: true);
        File.WriteAllText(SavePath(slot), json);
        Debug.Log($"Saved to {SavePath(slot)}");
    }

    public static SaveData LoadGame(int slot)
    {
        string path = SavePath(slot);
        if (!File.Exists(path))
        {
            Debug.LogWarning($"No save file found at {path}");
            return null;
        }

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<SaveData>(json);
    }
}