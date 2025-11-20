using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DIALOGUE;
using CHARACTERS;
using JetBrains.Annotations;

public class WorldManager : MonoBehaviour
{
    public GameObject characterPanel = null;
    public Room currentRoom;
    public RoomData currentRoomData;
    public bool isLoading = false;

    public GameObject charactersObject;
    public GameObject objectsObject;

    public static WorldManager instance { get; private set; }

    void Start()
    {
        instance = this;
        StartLoadingRoom(currentRoom, null);
    }

    private void ReturningToWorld()
    {
        StartCoroutine(ReturningToWorldInOrder());
    }

    IEnumerator ReturningToWorldInOrder()
    {
        if (ProgressManager.instance.currentGameEvent != null)
        {
            ProgressManager.instance.currentGameEvent.CheckIfFinished();
        }

        float timeOut = 0.5f;
        float elapsedTime = 0f;
        while (charactersObject != null && elapsedTime < timeOut)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    public void CreateCharacters(GameObject prefab)
    {
        if (prefab == null || characterPanel == null)
            return;

        GameObject ob = Instantiate(prefab, characterPanel.transform);
        ob.name = "Characters";
        ob.SetActive(true);
        foreach (string characterName in ProgressManager.instance.currentGameEvent.charactersData.Keys)
        {
            Transform character = ob.transform.Find(characterName);
            if (character != null)
                character.gameObject
                        .GetComponent<WorldCharacter>().isClicked =
                    ProgressManager.instance.currentGameEvent.charactersData[characterName].isClicked;
        }
        
        charactersObject = ob;

    }

    public void CreateObjects(GameObject prefab)
    {
        if (prefab == null || characterPanel == null)
            return;

        GameObject ob = Instantiate(prefab, characterPanel.transform);
        ob.name = "Objects";
        ob.SetActive(true);
        foreach (string objectName in ProgressManager.instance.currentGameEvent.objectsData.Keys)
        {
            ob.transform.Find(objectName).gameObject
                    .GetComponent<WorldObject>().isClicked =
                ProgressManager.instance.currentGameEvent.objectsData[objectName].isClicked;
        }

        objectsObject = ob;
    }

    public void HideCharacters()
    {
        Destroy(charactersObject);
    }

    public void StartLoadingRoom(Room room, [CanBeNull] string entryPoint)
    {
        StartCoroutine(LoadRoom(room, entryPoint));
    }

    private IEnumerator LoadRoom(Room room, [CanBeNull] string entryPoint)
    {
        CameraManager.instance?.StopAllPreviousOperations();

        isLoading = true;

        ImageScript.instance.FadeToBlack(0.2f);
        yield return new WaitForSeconds(0.2f);

        float timeout = 2f;
        float elapsedTime = 0f;

        currentRoom = Instantiate(room);
        currentRoom.name = room.name;
        currentRoomData =
            ProgressManager.instance.currentGameEvent.roomDatas.First(roomData => roomData.room.name == room.name);

        if (charactersObject != null)
        {
            foreach (Transform character in charactersObject.transform)
            {
                ProgressManager.instance.currentGameEvent.charactersData[character.name] =
                    new ObjectData(character.gameObject.GetComponent<WorldCharacter>().isClicked);
            }
        }

        GameObject world = GameObject.Find("World");
        Destroy(world);

        // Wait until World finished destroying (max 2 seconds to prevent infinite loops)
        while (world != null && elapsedTime < timeout)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }


        GameObject ob = Instantiate(room.prefab);
        ob.name = "World";
        ob.SetActive(true);
        
        GameObject objectsParent = GameObject.Find("World Objects");
        if (objectsParent != null)
            characterPanel = objectsParent;
        string cameraStartPosName = !String.IsNullOrEmpty(entryPoint) ? $":{entryPoint}" : "";
        Transform cameraStartPos = GameObject.Find($"World/CameraStartPos{cameraStartPosName}").transform;
        if (CameraManager.instance)
            CameraManager.instance.initialRotation =
                cameraStartPos
                    .rotation; // Sets only the Camera Manager's initial position value for later, not actually changing position of camera

        CharacterController controller = Camera.main.gameObject.GetComponent<CharacterController>();
        controller.enabled = false;
        CameraManager.instance.cameraTransform.position =
            cameraStartPos.position; // Actually changing position of camera
        CameraManager.instance.cameraTransform.rotation = cameraStartPos.rotation;
        controller.enabled = true;

        ImageScript.instance.UnFadeToBlack(0.1f);
        if (room.OnLoad() != null)
            yield return StartCoroutine(room.OnLoad());
        isLoading = false;

        CreateCharacters(ProgressManager.instance.currentGameEvent.roomDatas
            .First(roomData => roomData.room.name == currentRoom.name).characters);
        CreateObjects(ProgressManager.instance.currentGameEvent.roomDatas
            .First(roomData => roomData.room.name == currentRoom.name).worldObjects);

        yield return new WaitUntil(() =>
            (charactersObject != null || currentRoomData.characters == null) &&
            (objectsObject != null ||
             currentRoomData.worldObjects ==
             null)); // wait until both characters and objects loaded, or if there aren't any just go on

        ReturningToWorld();
    }

    public void HandleConversationEnd()
    {
        DialogueSystem.instance.SetIsActive(false);
        DialogueSystem.instance.ClearTextBox();
        CharacterClickEffects.instance.MakeCharactersReappear(charactersObject);
        currentRoom.OnConversationEnd();
        ReturningToWorld();
    }


    // Update is called once per frame
    void Update()
    {
        if (!DialogueSystem.instance.isActive && !PlayerInputManager.instance.isPaused && !isLoading)
            currentRoom.MovementControl();
        else
        {
            MapContainer.instance.HideMap();
        }
    }
}