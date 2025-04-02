using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;
using System.Threading.Tasks;

public class WorldManager : MonoBehaviour
{
    public GameObject characterPanel = null;
    public GameEvent currentGameEvent;
    public Room currentRoom;

    public static WorldManager instance { get; private set; }

    // Start is called before the first frame update
    async void Start()
    {
        instance = this;
        StartLoadingRoom(currentRoom);
        Dictionary<string, GameEvent> runtimeGameEvents = ProgressManager.instance.runtimeGameEvents;
        StartConversation("test");
        currentGameEvent = runtimeGameEvents["InsideRoom"];
    }

    void StartConversation(string textFile)
    {
        List<string> lines = FileManager.ReadTextAsset($"GameEvents/Story/{textFile}");

        DialogueSystem.instance.Say(lines);

    }

    public void ReturningToWorld()
    {
        if(currentGameEvent != null)
        {
            currentGameEvent.CheckIfFinished();
        }

        if(currentGameEvent != null)
        {
            currentGameEvent.PlayEvent();
        }
       
    }

    public void CreateCharacters(GameObject prefab)
    {
        GameObject ob = Instantiate(prefab, characterPanel.transform);
        ob.name = "Characters";
        ob.SetActive(true);
    }

    public void HideCharacters()
    {
        GameObject characters = GameObject.Find("World Objects/Characters");

        Destroy(characters);
    }

    public async void StartLoadingRoom(Room room)
    {
        await LoadRoom(room);
    }

    public async Task LoadRoom(Room room)
    {
        float timeout = 2f;
        float elapsedTime = 0f;

        currentRoom = Instantiate(room);
        currentRoom.name = room.name;
        Destroy(GameObject.Find("World"));

        // Wait until World finished destroying (max 2 seconds to prevent infinite loops)
        while (GameObject.Find("World") != null && elapsedTime < timeout)
        {
           await Task.Yield();
           elapsedTime += Time.deltaTime;
        }


        GameObject ob = Instantiate(room.prefab);
        ob.name = "World";
        ob.SetActive(true);

        elapsedTime = 0;
        // Wait until "World Objects" is found (max 2 seconds to prevent infinite loops)
        while (GameObject.Find("World/World Objects") == null && elapsedTime < timeout)
        {
           await Task.Yield();
           elapsedTime += Time.deltaTime;
        }
        
        if(GameObject.Find("World/World Objects") != null)
        characterPanel = GameObject.Find("World/World Objects");
        Transform cameraStartPos = GameObject.Find("World/CameraStartPos").transform;
        if(CameraManager.instance)
        CameraManager.instance.setInitialPosition(cameraStartPos.position, cameraStartPos.rotation); // Sets only the Camera Manager's initial position value for later, not actually changing position of camera

        Camera.main.transform.position = cameraStartPos.position; // Actually changing position of camera
        Camera.main.transform.rotation = cameraStartPos.rotation;

    }



    // Update is called once per frame
    void Update()
    {
        if(!DialogueSystem.instance.isActive)
        currentRoom.MovementControl();
    }
}
