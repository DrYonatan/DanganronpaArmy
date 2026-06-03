using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ObjectData
{
    public bool isClicked;
    public int clickCount;
    public bool isRequired;

    public ObjectData(bool isClicked, int clickCount)
    {
        this.isClicked = isClicked;
        this.clickCount = clickCount;
    }
}

[Serializable]
public class EventAdditionalObjectData
{
    public string id;
    public List<VNConversationSegment> texts;
}

[Serializable]
public class RoomData
{
    public Room room;
    public WorldCharactersParent characters;
    public GameObject worldObjects;
    public VNConversationSegment preLoadText;
    public bool isExitable;
    public List<EventAdditionalObjectData> additionalObjectData = new();
}

public abstract class WorldEvent : GameEvent
{
    public bool isFinished;

    public VNConversationSegment startText;
    public VNConversationSegment finishText;

    public bool isAfterStartText;
    public bool isAfterFinishText;

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

        WorldManager.instance.currentRoomData =
            roomDatas.Find(data => data.room.name.Equals(WorldManager.instance.currentRoom.name));
        isAfterStartText = true;

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
        }

        OnRoomLoad();
        WorldManager.instance.charactersObject?
            .AnimateCharacters();
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
            WorldManager.instance.StartCoroutine(WaitAndStartFinishText());
        }
        else
        {
            ProgressManager.instance.OnEventFinished();
        }
    }

    private IEnumerator WaitAndStartFinishText()
    {
        yield return new WaitUntil(() => CameraManager.instance.isInFinalRotation); // Wait until camera finishes moving
        VNNodePlayer.instance.StartConversation(finishText);
        isAfterFinishText = true;
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
            {
                WorldCharacter worldCharacter = character.GetComponent<WorldCharacter>();
                worldCharacter.isClicked = charactersData[characterName].isClicked;
                worldCharacter.clickCount = charactersData[characterName].clickCount;
            }
        }

        WorldManager.instance.charactersObject = ob;

        foreach (Transform character in WorldManager.instance.charactersObject.transform)
        {
            WorldCharacter worldCharacter = character.GetComponent<WorldCharacter>();
            charactersData[character.name] =
                new ObjectData(worldCharacter.isClicked, worldCharacter.clickCount);
        }
    }

    private void CreateObjects(GameObject prefab)
    {
        if (WorldManager.instance.characterPanel == null)
            return;

        GameObject ob = null;
        if (prefab != null)
        {
            ob = Instantiate(prefab, WorldManager.instance.characterPanel.transform);
            ob.name = "Objects";
            ob.SetActive(true);
        }

        foreach (string objectName in objectsData.Keys)
        {
            Transform objectTransform = ob?.transform.Find(objectName);

            if (objectTransform == null)
                objectTransform = WorldManager.instance.currentRoomModel.interactables
                    .Find(x => x.gameObject.name == objectName)
                    ?.transform;

            if (objectTransform != null)
            {
                WorldObject worldObject = objectTransform.GetComponent<WorldObject>();
                worldObject.isClicked = objectsData[objectName].isClicked;
                worldObject.clickCount = objectsData[objectName].clickCount;
            }
        }

        if (ob != null)
        {
            WorldManager.instance.objectsObject = ob;

            foreach (Transform obj in WorldManager.instance.objectsObject.transform) // Add all event objects to dictionary
            {
                WorldObject worldObject = obj.GetComponent<WorldObject>();

                objectsData[obj.name] =
                    new ObjectData(worldObject.isClicked, worldObject.clickCount);
            }
        }

        foreach (ConversationInteractable interactable in
                 WorldManager.instance.currentRoomModel
                     .interactables) // Add all normal room interactables to the dictionary as well
        {
            objectsData[interactable.name] =
                new ObjectData(interactable.isClicked, interactable.clickCount);
        }
    }

    public override void LoadSave(SaveData data)
    {
        base.LoadSave(data);
        isAfterFinishText = data.isAfterFinishText;
        charactersData = data.charactersData.ToDictionary(c => c.key, c => c.value);
        objectsData = data.objectsData.ToDictionary(c => c.key, c => c.value);
    }

    protected void OnNotFinished()
    {
        CursorManager.instance.Show();
    }
}