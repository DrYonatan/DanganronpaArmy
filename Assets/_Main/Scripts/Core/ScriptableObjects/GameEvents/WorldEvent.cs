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

public abstract class WorldEvent : GameEvent
{
    public bool isFinished;

    public VNConversationSegment startText;
    public VNConversationSegment finishText;

    private bool isAfterFinishText;

    public VNConversationSegment unallowedText;

    public Dictionary<string, ObjectData> charactersData = new Dictionary<string, ObjectData>();

    public Dictionary<string, ObjectData> objectsData = new Dictionary<string, ObjectData>();

    public Room startRoom;

    public override void OnStart()
    {
        WorldManager.instance.StartCoroutine(OnStartRoutine());
    }

    private IEnumerator OnStartRoutine()
    {
        yield return StartWithRoomLoad();

        if (startText != null)
        {
            VNNodePlayer.instance.StartConversation(startText);
        }
        else
            CursorManager.instance.Show();
    }

    private IEnumerator StartWithRoomLoad()
    {
        Room roomToLoad;

        if (startRoom != null &&
            WorldManager.instance.currentRoom?.roomName != startRoom.roomName)
            roomToLoad = startRoom;
        else
        {
            roomToLoad = WorldManager.instance.currentRoom;
        }

        if (WorldManager.instance.currentTime != timeOfDay ||
            roomToLoad != WorldManager.instance.currentRoom)
        {
            WorldManager.instance.currentRoom = roomToLoad;
            yield return TimeOfDayManager.instance.ChangeTimeOfDay(timeOfDay);
            yield return WorldManager.instance.LoadRoom(WorldManager.instance.currentRoom, null);
            OnRoomLoad();
            WorldManager.instance.charactersObject?
                .AnimateCharacters();
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

    public override void OnRoomLoad()
    {
        RoomData currentRoomData = roomDatas
            .First(roomData => roomData.room.roomName.Equals(WorldManager.instance.currentRoom.roomName));

        if (WorldManager.instance.charactersObject == null)
        {
            CreateCharacters(currentRoomData.characters);
        }

        if (WorldManager.instance.objectsObject == null)
            CreateObjects(currentRoomData.worldObjects);

        WorldManager.instance.UpdateRoomData(currentRoomData);
    }

    private void CreateCharacters(WorldCharactersParent prefab)
    {
        if (prefab == null || WorldManager.instance.characterPanel == null)
            return;

        WorldCharactersParent ob = Instantiate(prefab, WorldManager.instance.characterPanel.transform);
        ob.name = "Characters";
        ob.gameObject.SetActive(true);
        foreach (string characterName in charactersData.Keys)
        {
            Transform character = ob.transform.Find(characterName);
            if (character != null)
                character.gameObject
                        .GetComponent<WorldCharacter>().isClicked =
                    charactersData[characterName].isClicked;
        }

        WorldManager.instance.charactersObject = ob;

        foreach (Transform character in WorldManager.instance.charactersObject.transform)
        {
            charactersData[character.name] =
                new ObjectData(character.gameObject.GetComponent<WorldCharacter>().isClicked);
        }
    }

    private void CreateObjects(GameObject prefab)
    {
        if (prefab == null || WorldManager.instance.characterPanel == null)
            return;

        GameObject ob = Instantiate(prefab, WorldManager.instance.characterPanel.transform);
        ob.name = "Objects";
        ob.SetActive(true);
        foreach (string objectName in objectsData.Keys)
        {
            ob.transform.Find(objectName).gameObject
                    .GetComponent<WorldObject>().isClicked =
                objectsData[objectName].isClicked;
        }

        WorldManager.instance.objectsObject = ob;

        foreach (Transform obj in WorldManager.instance.objectsObject.transform)
        {
            objectsData[obj.name] =
                new ObjectData(obj.gameObject.GetComponent<WorldObject>().isClicked);
        }
    }

    public override void LoadSave(SaveData data)
    {
        base.LoadSave(data);
        charactersData = data.charactersData.ToDictionary(c => c.key, c => c.value);
        objectsData = data.objectsData.ToDictionary(c => c.key, c => c.value);
    }
}