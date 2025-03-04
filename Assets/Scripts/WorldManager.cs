using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

public class WorldManager : MonoBehaviour
{
    public RectTransform characterPanel = null;
    public GameEvent currentGameEvent;

    public static WorldManager instance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        StartConversation("Prologue2");
    }

    void StartConversation(string textFile)
    {
        List<string> lines = FileManager.ReadTextAsset($"Scenes/Story/{textFile}");

        DialogueSystem.instance.Say(lines);

    }

    public void ReturningToWorld()
    {
        GameObject dialogueBox = GameObject.Find("VN controller/Root/Canvas - Main/LAYERS/4 - Dialogue");

        if (currentGameEvent.CheckIfFinished())
        {
            currentGameEvent.OnFinish();
        }
        
        else
        dialogueBox.SetActive(false);


        if(currentGameEvent != null)
        {
            currentGameEvent.PlayEvent();
        }
       
    }

    public void CreateCharacters(GameObject prefab)
    {
        Debug.Log("Working well");
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
        
    }
}
