using System;
using System.Collections;
using UnityEngine;
using DIALOGUE;
using JetBrains.Annotations;

public class WorldManager : MonoBehaviour
{
    public GameObject characterPanel = null;
    public Room currentRoom;
    public RoomData currentRoomData;
    public bool isLoading = false;

    public WorldCharactersParent charactersObject;
    public GameObject objectsObject;

    public TimeOfDay currentTime;

    public static WorldManager instance { get; private set; }

    void Awake()
    {
        instance = this;
    }

    public void Initialize()
    {
        StartCoroutine(LoadRoomWithoutAnimation(currentRoom));
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

    public void StartLoadingRoom(Room room, [CanBeNull] string entryPoint)
    {
        StartCoroutine(MoveToRoom(room, entryPoint));
    }

    public void UpdateRoomData(RoomData roomData)
    {
        currentRoomData = roomData;
    }

    public IEnumerator LoadRoom(Room room, [CanBeNull] string entryPoint)
    {
        ImageScript.instance.HideBackground();

        isLoading = true;

        RoomModel ob = Instantiate(room.GetTimeOfDayVersion(ProgressManager.instance.currentGameEvent.timeOfDay));
        ob.name = "World";
        ob.gameObject.SetActive(true);

        GameObject objectsParent = GameObject.Find("World Objects");
        if (objectsParent != null)
            characterPanel = objectsParent;
        string cameraStartPosName = !String.IsNullOrEmpty(entryPoint) ? $":{entryPoint}" : "";
        Transform cameraStartPos = GameObject.Find($"World/CameraStartPos{cameraStartPosName}").transform;
        if (CameraManager.instance)
            CameraManager.instance.initialRotation =
                cameraStartPos
                    .rotation; // Sets only the Camera Manager's initial position value for later, not actually changing position of camera

        CameraManager.instance.player.enabled = false;
        CameraManager.instance.player.transform.position =
            cameraStartPos.position; // Actually changing position of camera
        CameraManager.instance.cameraTransform.rotation = cameraStartPos.rotation;
        CameraManager.instance.player.enabled = true;

        ImageScript.instance.UnFadeToBlack(0.1f);
        if (room.OnLoad() != null)
            yield return StartCoroutine(room.OnLoad());
        ob.PlayRoomIntroEffects();
        yield return room.AppearAnimation();
        isLoading = false;
        PlayerInputManager.instance.EnableInput();
    }
    
    private IEnumerator LoadRoomWithoutAnimation(Room room)
    {
        RoomModel ob = Instantiate(room.GetTimeOfDayVersion(ProgressManager.instance.currentGameEvent.timeOfDay));
        ob.name = "World";
        ob.gameObject.SetActive(true);
        
        ImageScript.instance.UnFadeToBlack(0.2f);

        GameObject objectsParent = GameObject.Find("World Objects");
        if (objectsParent != null)
            characterPanel = objectsParent;

        if (room.OnLoad() != null)
            yield return StartCoroutine(room.OnLoad());
        isLoading = false;

        ProgressManager.instance.currentGameEvent.OnRoomLoad();
        
        if (VNNodePlayer.instance.currentConversation == null)
        {
            ReturningToWorld();
        }
        else
        {
            CharacterClickEffects.instance.MakeCharactersDisappear(charactersObject, 0f);
        }
    }

    private IEnumerator MoveToRoom(Room room, [CanBeNull] string entryPoint)
    {
        CameraManager.instance.footStepsSource.Stop();
        CursorManager.instance.ShowOrHideConversationIcon(false);
        CursorManager.instance.ShowOrHideInteractableName(false, "");

        CameraManager.instance?.StopAllPreviousOperations();

        isLoading = true;

        ImageScript.instance.FadeToBlack(0.2f);
        yield return new WaitForSeconds(0.2f);

        float timeout = 2f;
        float elapsedTime = 0f;

        currentRoom = Instantiate(room);
        currentRoom.name = room.name;
        currentRoom.roomName = room.roomName;
        
        GameObject world = GameObject.Find("World");
        Destroy(world);

        // Wait until World finished destroying (max 2 seconds to prevent infinite loops)
        while (world != null && elapsedTime < timeout)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return LoadRoom(room, entryPoint);

        ProgressManager.instance.currentGameEvent.OnRoomLoad();

        if (charactersObject != null)
            charactersObject.AnimateCharacters();

        CursorManager.instance.Show();

        ReturningToWorld();
    }

    public void HandleConversationEnd()
    {
        DialogueSystem.instance.SetIsActive(false);
        DialogueSystem.instance.ClearTextBox();
        if (charactersObject != null)
            CharacterClickEffects.instance.MakeCharactersReappear(charactersObject.gameObject);
        if(currentRoom != null)
           currentRoom.OnConversationEnd();
        ReturningToWorld();
    }

    // Update is called once per frame
    void Update()
    {
        if (!DialogueSystem.instance.isActive && !PlayerInputManager.instance.isPaused && !isLoading)
            currentRoom?.MovementControl();
        else
        {
            MapContainer.instance.HideMap();
        }
    }
}