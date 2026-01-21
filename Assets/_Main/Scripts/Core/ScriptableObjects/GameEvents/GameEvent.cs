using System;
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

    public VNConversationSegment startText;
    public VNConversationSegment finishText;

    public bool isAfterFinishText;

    public VNConversationSegment unallowedText;

    public List<RoomData> roomDatas;

    public Dictionary<string, ObjectData> charactersData = new Dictionary<string, ObjectData>();

    public Dictionary<string, ObjectData> objectsData = new Dictionary<string, ObjectData>();

    public TimeOfDay timeOfDay;

    public Room startRoom;
    public abstract void CheckIfFinished();

    public virtual void OnStart()
    {
        RoomData currentRoomData = ProgressManager.instance.currentGameEvent.roomDatas
            .First(roomData => roomData.room.roomName.Equals(WorldManager.instance.currentRoom.roomName));

        WorldManager.instance.UpdateRoomData(currentRoomData);

        if (WorldManager.instance.charactersObject == null)
        {
            WorldManager.instance.CreateCharacters(currentRoomData.characters);
            WorldManager.instance.charactersObject.AnimateCharacters();
        }

        if (WorldManager.instance.objectsObject == null)
            WorldManager.instance.CreateObjects(currentRoomData.worldObjects);

        if (startText != null)
        {
            VNNodePlayer.instance.StartConversation(startText);
        }
    }

    protected virtual void OnFinish()
    {
        if (WorldManager.instance.charactersObject != null)
            Destroy(WorldManager.instance.charactersObject.gameObject);
        if (WorldManager.instance.objectsObject != null)
            Destroy(WorldManager.instance.objectsObject.gameObject);
        WorldManager.instance.charactersObject = null;
        WorldManager.instance.objectsObject = null;
        
        if (finishText != null && !isAfterFinishText)
        {
            VNNodePlayer.instance.StartConversation(finishText);
            isAfterFinishText = true;
        }
        else
        {
            ProgressManager.instance.OnEventFinished();
        }
    }
}