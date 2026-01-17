using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    public int chapterIndex;
    public int chapterSegmentIndex;
    
    // VN stuff
    public int gameEventIndex;
    public string currentRoom;
    public string currentConversation;
    public int currentLineIndex;
    public string currentMusic;
    public List<ObjectDataEntry> charactersData;
    public List<ObjectDataEntry> objectsData;
    public string scene;
    public List<CharacterRankEntry> characterRanks;
    public float[] playerPosition;
    public float[] cameraPosition;
    public float[] cameraRotation;
    public float[] conversationInitialRotation;
    public TimeOfDay timeOfDay;

    // Trial stuff
    public int trialSegmentIndex;
    public float hp;

    public SaveData(int chapterIndex, int chapterSegmentIndex, int gameEventIndex, string currentRoom, string currentConversation,
        int currentLineIndex, string currentMusic, Dictionary<string, ObjectData> charactersData,
        Dictionary<string, ObjectData> objectsData,
        string scene,
        Dictionary<string, int> characterRanks, Vector3 playerPosition, Vector3 cameraPosition, Vector3 cameraRotation,
        Vector3 conversationInitialRotation, TimeOfDay timeOfDay, int trialSegmentIndex,
        float hp)
    {
        this.chapterIndex = chapterIndex;
        this.chapterSegmentIndex = chapterSegmentIndex;
        this.gameEventIndex = gameEventIndex;
        this.currentRoom = currentRoom;
        this.currentConversation = currentConversation;
        this.currentLineIndex = currentLineIndex;
        this.currentMusic = currentMusic;
        this.scene = scene;
        if (objectsData != null)
            this.objectsData = objectsData
                .Select(kvp => new ObjectDataEntry { key = kvp.Key, value = kvp.Value })
                .ToList();

        if (charactersData != null)
            this.charactersData = charactersData.Select(kvp => new ObjectDataEntry { key = kvp.Key, value = kvp.Value })
                .ToList();
        this.characterRanks = characterRanks
            .Select(kvp => new CharacterRankEntry { key = kvp.Key, value = kvp.Value })
            .ToList();

        this.playerPosition = new float[3];
        this.cameraPosition = new float[3];
        this.cameraRotation = new float[3];
        this.conversationInitialRotation = new float[3];
        
        this.playerPosition[0] = playerPosition.x;
        this.playerPosition[1] = playerPosition.y;
        this.playerPosition[2] = playerPosition.z;

        this.cameraPosition[0] = cameraPosition.x;
        this.cameraPosition[1] = cameraPosition.y;
        this.cameraPosition[2] = cameraPosition.z;

        this.cameraRotation[0] = cameraRotation.x;
        this.cameraRotation[1] = cameraRotation.y;
        this.cameraRotation[2] = cameraRotation.z;
        
        this.conversationInitialRotation[0] = conversationInitialRotation.x;
        this.conversationInitialRotation[1] = conversationInitialRotation.y;
        this.conversationInitialRotation[2] = conversationInitialRotation.z;
        
        this.timeOfDay = timeOfDay;

        this.trialSegmentIndex = trialSegmentIndex;
        this.hp = hp;
    }
}