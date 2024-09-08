using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

public class WorldObjectsInteraction : MonoBehaviour
{
    public string characterName;
    public bool isClicked = false;

    // Start is called before the first frame update
    void Start()
    {
  
       
    }


    

    private void OnMouseDown()
    {

        GameObject dialogueBox = GameObject.Find("VN controller/Root/Canvas - Overlay/4 - Dialogue");
        dialogueBox.SetActive(true);
        CameraManager.instance.ZoomCamera("in");
        if(WorldManager.instance.currentScene != null)
        gameObject.transform.parent.transform.localScale = new Vector3(0, 0, 0);
        StartConversation();
        WorldManager.instance.currentScene.UpdateScene();
    }


    void StartConversation()
    {
        List<string> lines;
        string currentSceneName = WorldManager.instance.currentScene.name;
        if (!isClicked)
        {
            lines = FileManager.ReadTextAsset($"Scenes/{currentSceneName}/{characterName}");
        }
        else
        {
            lines = FileManager.ReadTextAsset($"Scenes/{currentSceneName}/{characterName}2");
        }
        DialogueSystem.instance.Say(lines);
        isClicked = true;

    }
}
