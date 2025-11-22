using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class ObjectDataEntry
{
    public string key;
    public ObjectData value;
}

[Serializable]
public class CharacterRankEntry
{
    public string key;
    public int value;
}

[Serializable]
public class SaveData
{
    public int gameEventIndex;
    public string currentRoom;
    public string currentConversation;
    public int currentLineIndex;
    public string currentMusic;
    public List<ObjectDataEntry> charactersData;
    public List<ObjectDataEntry> objectsData;
    public string scene;
    public List<CharacterRankEntry> characterRanks;

    public SaveData(int gameEventIndex, string currentRoom, string currentConversation,
        int currentLineIndex, string currentMusic, Dictionary<string, ObjectData> charactersData,
        Dictionary<string, ObjectData> objectsData,
        string scene,
        Dictionary<string, int> characterRanks)
    {
        this.gameEventIndex = gameEventIndex;
        this.currentRoom = currentRoom;
        this.currentConversation = currentConversation;
        this.currentLineIndex = currentLineIndex;
        this.currentMusic = currentMusic;
        this.scene = scene;
        this.objectsData = objectsData
            .Select(kvp => new ObjectDataEntry { key = kvp.Key, value = kvp.Value })
            .ToList();
        this.charactersData = charactersData.Select(kvp => new ObjectDataEntry { key = kvp.Key, value = kvp.Value })
            .ToList();
        this.characterRanks = characterRanks
            .Select(kvp => new CharacterRankEntry { key = kvp.Key, value = kvp.Value })
            .ToList();
    }
}