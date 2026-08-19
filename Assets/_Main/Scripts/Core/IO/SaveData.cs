using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
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
public class EventState
{
    public bool isFinished;

    public EventState(bool isFinished)
    {
        this.isFinished = isFinished;
    }
}

[Serializable]
public class WorldEventState : EventState
{
    public List<ObjectDataEntry> charactersData;
    public List<ObjectDataEntry> objectsData;
    public bool isAfterFinishText;
    public List<RoomDataSave> roomsDatas;

    public WorldEventState(bool isFinished, Dictionary<string, ObjectData> objectsData,
        Dictionary<string, ObjectData> charactersData, bool isAfterFinishText,
        List<RoomData> roomDatas) : base(isFinished)
    {
        if (objectsData != null)
            this.objectsData = objectsData
                .Select(kvp => new ObjectDataEntry { key = kvp.Key, value = kvp.Value })
                .ToList();

        if (charactersData != null)
            this.charactersData = charactersData.Select(kvp => new ObjectDataEntry { key = kvp.Key, value = kvp.Value })
                .ToList();

        this.isAfterFinishText = isAfterFinishText;

        this.roomsDatas = roomDatas.ConvertAll(data => new RoomDataSave(data.room.name, data.hasAlreadyEntered));
    }

    [JsonConstructor]
    public WorldEventState(bool isFinished) : base(isFinished)
    {
        charactersData = new List<ObjectDataEntry>();
        objectsData = new List<ObjectDataEntry>();
        roomsDatas = new List<RoomDataSave>();
    }
}

[Serializable]
public class RoomDataSave
{
    public string roomName;
    public bool hasAlreadyEntered;

    public RoomDataSave(string roomName, bool hasAlreadyEntered)
    {
        this.roomName = roomName;
        this.hasAlreadyEntered = hasAlreadyEntered;
    }
}

[Serializable]
public class SaveData
{
    public int chapterIndex;
    public int chapterSegmentIndex;

    // VN stuff
    public int gameEventIndex;
    public string currentRoom;
    public bool savedInPopup;
    public string currentConversation;
    public int currentLineIndex;
    public string currentMusic;
    public EventState eventState;
    public string scene;
    public List<CharacterRankEntry> characterRanks;
    public float[] playerPosition;
    public float[] cameraPosition;
    public float[] cameraRotation;
    public float[] conversationInitialRotation;
    public TimeOfDay timeOfDay;
    public UIState uiState;
    public List<string> evidenceIds;

    // Trial stuff
    public int trialSegmentIndex;
    public float hp;

    public string saveTime;

    public SaveData(int chapterIndex, int chapterSegmentIndex, int gameEventIndex, string currentRoom,
        bool savedInPopup, string currentConversation,
        int currentLineIndex, string currentMusic, EventState eventState,
        string scene,
        Dictionary<string, int> characterRanks, Vector3 playerPosition, Vector3 cameraPosition, Vector3 cameraRotation,
        Vector3 conversationInitialRotation, TimeOfDay timeOfDay, UIState uiState, List<string> evidenceIds,
        int trialSegmentIndex,
        float hp, string saveTime)
    {
        this.chapterIndex = chapterIndex;
        this.chapterSegmentIndex = chapterSegmentIndex;
        this.gameEventIndex = gameEventIndex;
        this.currentRoom = currentRoom;
        this.savedInPopup = savedInPopup;
        this.currentConversation = currentConversation;
        this.currentLineIndex = currentLineIndex;
        this.currentMusic = currentMusic;
        this.scene = scene;
        this.eventState = eventState;
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
        this.uiState = uiState;

        this.evidenceIds = evidenceIds;

        this.trialSegmentIndex = trialSegmentIndex;
        this.hp = hp;

        this.saveTime = saveTime;
    }

    public SaveData()
    {
        chapterIndex = 0;
        chapterSegmentIndex = 0;
        gameEventIndex = 0;
        currentRoom = "";
        savedInPopup = false;
        currentConversation = "";
        currentLineIndex = 0;
        currentMusic = "";
        scene = "";
        eventState = null;
        characterRanks = new List<CharacterRankEntry>();

        playerPosition = new float[3];
        cameraPosition = new float[3];
        cameraRotation = new float[3];
        conversationInitialRotation = new float[3];

        playerPosition[0] = 0;
        playerPosition[1] = 0;
        playerPosition[2] = 0;

        cameraPosition[0] = 0;
        cameraPosition[1] = 0;
        cameraPosition[2] = 0;

        cameraRotation[0] = 0;
        cameraRotation[1] = 0;
        cameraRotation[2] = 0;

        conversationInitialRotation[0] = 0;
        conversationInitialRotation[1] = 0;
        conversationInitialRotation[2] = 0;

        timeOfDay = 0;
        uiState = null;

        evidenceIds = null;

        trialSegmentIndex = 0;
        hp = 0;

        saveTime = "";
    }
}