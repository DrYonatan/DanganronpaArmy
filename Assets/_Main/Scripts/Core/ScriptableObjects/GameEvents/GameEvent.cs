using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public abstract void CheckIfFinished();

    public virtual void OnStart()
    {
        string sceneName = TimeOfDayManager.instance.GetTimeScene(timeOfDay);
        if (timeOfDay != WorldManager.instance.currentTime)
        {
            WorldManager.instance.currentTime = timeOfDay;
            SceneManager.LoadScene(sceneName);
            WorldManager.instance.StartLoadingRoom(WorldManager.instance.currentRoom, null);
        }

        RoomData currentRoomData = ProgressManager.instance.currentGameEvent.roomDatas
            .First(roomData => roomData.room.name.Equals(WorldManager.instance.currentRoom.name));

        WorldManager.instance.UpdateRoomData(currentRoomData);

        if (WorldManager.instance.charactersObject == null)
        {
            WorldManager.instance.CreateCharacters(currentRoomData.characters);
            WorldManager.instance.charactersObject.AnimateCharacters();
        }

        if (WorldManager.instance.objectsObject == null)
            WorldManager.instance.CreateObjects(currentRoomData.worldObjects);
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