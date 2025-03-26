using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;
using System.Threading.Tasks;

public class WorldManager : MonoBehaviour
{
    public RectTransform characterPanel = null;
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
           currentGameEvent.CheckIfFinished();

        DialogueSystem.instance.isActive = false;
        GameObject dialogueBox = GameObject.Find("VN controller/Root/Canvas - Main/LAYERS/4 - Dialogue");
        dialogueBox.GetComponent<CanvasGroup>().alpha = 0;


        if(currentGameEvent != null)
        {
            currentGameEvent.PlayEvent();
        }
       
    }

    public void CreateCharacters(GameObject prefab)
    {
        GameObject ob = Instantiate(prefab, characterPanel);
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
        
        if(GameObject.Find("World/World Objects").GetComponent<RectTransform>() != null)
        characterPanel = GameObject.Find("World/World Objects").GetComponent<RectTransform>();
        Transform cameraStartPos = GameObject.Find("World/CameraStartPos").transform;
        Camera.main.transform.position = cameraStartPos.position;
        Camera.main.transform.rotation = cameraStartPos.rotation;
        CameraManager.instance.setInitialPosition(cameraStartPos);

    }



    // Update is called once per frame
    void Update()
    {
        if(!DialogueSystem.instance.isActive)
        currentRoom.MovementControl();
    }
}
