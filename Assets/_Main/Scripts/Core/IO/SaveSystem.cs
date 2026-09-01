using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class SaveSystem
{
    private static string SavePath(int slot) =>
        Path.Combine(Application.persistentDataPath, $"save_{slot}.json");

    public static void SaveGame(SaveData data, int slot)
    {
        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        };
        
        string json = JsonConvert.SerializeObject(data, Formatting.Indented, settings);
        File.WriteAllText(SavePath(slot), json);
    }

    public static SaveData LoadGame(int slot)
    {
        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        };
        
        string path = SavePath(slot);
        if (!File.Exists(path))
        {
            return null;
        }
      

        string json = File.ReadAllText(path);
        SaveData data = JsonConvert.DeserializeObject<SaveData>(json, settings);

        if (data != null && !HasPauseAvailable(json))
            data.pauseAvailable = true;

        return data;
    }

    private static bool HasPauseAvailable(string json)
    {
        try
        {
            return JObject.Parse(json).ContainsKey(nameof(SaveData.pauseAvailable));
        }
        catch
        {
            return false;
        }
    }
}