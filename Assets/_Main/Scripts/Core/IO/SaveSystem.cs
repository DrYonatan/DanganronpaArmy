using System.IO;
using Newtonsoft.Json;
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
        return JsonConvert.DeserializeObject<SaveData>(json, settings);
    }
}