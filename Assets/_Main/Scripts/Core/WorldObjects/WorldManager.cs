using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;
using CHARACTERS;
using JetBrains.Annotations;

public class WorldManager : MonoBehaviour
{
    public GameObject characterPanel = null;
    public GameEvent currentGameEvent;
    public Room currentRoom;
    public bool isLoading = false;

    public static WorldManager instance { get; private set; }

    // Start is called before the first frame update
     void Start()
    {
        instance = this;
        StartLoadingRoom(currentRoom, null);
        Dictionary<string, GameEvent> runtimeGameEvents = ProgressManager.instance.runtimeGameEvents;
        currentGameEvent = runtimeGameEvents["InsideRoom"];
        ReturningToWorld();
    }

    public void ReturningToWorld()
    {
        StartCoroutine(ReturningToWorldInOrder());
    }

    IEnumerator ReturningToWorldInOrder()
    {
        if(currentGameEvent != null)
        {
            currentGameEvent.UpdateEvent();
            currentGameEvent.CheckIfFinished();
        }

        float timeOut = 0.5f;
        float elapsedTime = 0f;
        while(GameObject.Find("World/World Objects/Characters") != null && elapsedTime < timeOut)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        ProgressManager.instance.DecideWhichSceneToPlay();

        if(currentGameEvent != null)
        {
            currentGameEvent.UpdateEvent();
        }  
        yield return null;
    }

    public void CreateCharacters(GameObject prefab)
    {
        GameObject ob = Instantiate(prefab, characterPanel.transform);
        ob.name = "Characters";
        ob.SetActive(true);
        foreach(string characterName in currentGameEvent.charactersData.Keys) 
        {
            Transform character = ob.transform.Find(characterName);
            if(character != null)
            character.gameObject
            .GetComponent<WorldCharacter>().isClicked =
             currentGameEvent.charactersData[characterName].isClicked;
        }
    }

    public void CreateObjects(GameObject prefab)
    {
        GameObject ob = Instantiate(prefab, characterPanel.transform);
        ob.name = "Objects";
        ob.SetActive(true);
        foreach(string objectName in currentGameEvent.objectsData.Keys) 
        {
            ob.transform.Find(objectName).gameObject
            .GetComponent<WorldObject>().isClicked =
             currentGameEvent.objectsData[objectName].isClicked;
        }
    }

    public void HideCharacters()
    {
        GameObject characters = GameObject.Find("World Objects/Characters");

        Destroy(characters);
    }

    public void StartLoadingRoom(Room room, [CanBeNull] string entryPoint)
    {
        StartCoroutine(LoadRoom(room, entryPoint));
    }

    public IEnumerator LoadRoom(Room room, [CanBeNull] string entryPoint)
    {
         CameraManager.instance?.StopAllPreviousOperations();

        isLoading = true;
        
        ImageScript.instance.FadeToBlack(0.2f);
        yield return new WaitForSeconds(0.2f);
        
        float timeout = 2f;
        float elapsedTime = 0f;

        currentRoom = Instantiate(room);
        currentRoom.name = room.name;
        Transform characters = GameObject.Find("World/World Objects/Characters")?.transform;

        if (characters != null)
        {
           foreach (Transform character in characters)
          {
            currentGameEvent.charactersData[character.name] = 
            new ObjectData(character.gameObject.GetComponent<WorldCharacter>().isClicked);
          }
        }

        
        Destroy(GameObject.Find("World"));

        // Wait until World finished destroying (max 2 seconds to prevent infinite loops)
        while (GameObject.Find("World") != null && elapsedTime < timeout)
        {
           elapsedTime += Time.deltaTime;
           yield return null;
        }


        GameObject ob = Instantiate(room.prefab);
        ob.name = "World";
        ob.SetActive(true);

        elapsedTime = 0;
        // Wait until "World Objects" is found (max 2 seconds to prevent infinite loops)
        while (GameObject.Find("World/World Objects") == null && elapsedTime < timeout)
        {
           elapsedTime += Time.deltaTime;
           yield return null;
        }
        
        if(GameObject.Find("World/World Objects") != null)
        characterPanel = GameObject.Find("World/World Objects");
        string cameraStartPosName = !String.IsNullOrEmpty(entryPoint) ? $":{entryPoint}" : "";
        Transform cameraStartPos = GameObject.Find($"World/CameraStartPos{cameraStartPosName}").transform;
        if(CameraManager.instance)
        CameraManager.instance.initialRotation = cameraStartPos.rotation; // Sets only the Camera Manager's initial position value for later, not actually changing position of camera
        
        CharacterController controller = Camera.main.gameObject.GetComponent<CharacterController>();
        controller.enabled = false;
        Camera.main.transform.position = cameraStartPos.position; // Actually changing position of camera
        Camera.main.transform.rotation = cameraStartPos.rotation;
        controller.enabled = true;
        
        ImageScript.instance.UnFadeToBlack(0.1f);
        if(room.OnLoad() != null)
        yield return StartCoroutine(room.OnLoad());
        isLoading = false;
        ReturningToWorld();
    }

    public void HandleConversationEnd()
    {
        DialogueSystem.instance.SetIsActive(false);
        currentGameEvent.UpdateEvent();
        GameObject characters = GameObject.Find("World/World Objects/Characters");
        if(characters != null)
        CharacterClickEffects.instance.MakeCharactersReappear(characters);
        ReturningToWorld();
        DialogueSystem.instance.ClearTextBox();
        if(currentRoom is PointAndClickRoom && !currentGameEvent.isFinished)
        CameraManager.instance.ReturnToDollyTrack();
    }


    // Update is called once per frame
    void Update()
    {
        if(!DialogueSystem.instance.isActive && !PlayerInputManager.instance.isPaused && !isLoading)
           currentRoom.MovementControl();
        else
        {
            MapContainer.instance.HideMap();
        }
    }

}
