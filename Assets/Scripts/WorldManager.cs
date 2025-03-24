using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

public class WorldManager : MonoBehaviour
{
    public RectTransform characterPanel = null;
    public GameEvent currentGameEvent;
    public Room currentRoom;

    public static WorldManager instance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        currentRoom = Instantiate(currentRoom);
        Dictionary<string, GameEvent> runtimeGameEvents = ProgressManager.instance.runtimeGameEvents;
        StartConversation("test");
        currentGameEvent = runtimeGameEvents["Scene1"];
        characterPanel = GameObject.Find("World").transform.GetChild(0).transform.Find("World Objects").GetComponent<RectTransform>();
    }

    void StartConversation(string textFile)
    {
        List<string> lines = FileManager.ReadTextAsset($"GameEvents/Story/{textFile}");

        DialogueSystem.instance.Say(lines);

    }

    public void ReturningToWorld()
    {
        currentGameEvent.CheckIfFinished();
        DialogueSystem.instance.isActive = false;

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

    // Update is called once per frame
    void Update()
    {
        if(!DialogueSystem.instance.isActive)
        currentRoom.MovementControl();
    }
}
