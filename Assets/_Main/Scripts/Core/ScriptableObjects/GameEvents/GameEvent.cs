using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ObjectData
{
    public bool isClicked;

    public ObjectData(bool isClicked)
    {
        this.isClicked = isClicked;
    }
}

[Serializable]
public class RoomData
{
    public Room room;
    public WorldCharactersParent characters;
    public GameObject worldObjects;
    public bool isExitable;
}

public abstract class GameEvent : ScriptableObject
{
    public bool isFinished;

    public bool
        startEventImmediately =
            false; // used to know if to start the event as soon as the previous one ends or only after finish text

    public VNConversationSegment finishText;

    public VNConversationSegment unallowedText;

    public List<RoomData> roomDatas;

    public Dictionary<string, ObjectData> charactersData = new Dictionary<string, ObjectData>();

    public Dictionary<string, ObjectData> objectsData = new Dictionary<string, ObjectData>();

    public abstract void CheckIfFinished();

    public virtual void OnStart()
    {
        RoomData currentRoomData = ProgressManager.instance.currentGameEvent.roomDatas
            .First(roomData => roomData.room.name.Equals(WorldManager.instance.currentRoom.name));
        
        WorldManager.instance.UpdateRoomData(currentRoomData);
        
        if (WorldManager.instance.charactersObject == null)
            WorldManager.instance.CreateCharacters(currentRoomData.characters);
        if (WorldManager.instance.objectsObject == null)
            WorldManager.instance.CreateObjects(currentRoomData.worldObjects);
    }

    protected virtual void OnFinish()
    {
        if (finishText != null)
        {
            VNNodePlayer.instance.StartConversation(finishText);
            finishText = null;
        }

        ProgressManager.instance.OnEventFinished();
    }
}