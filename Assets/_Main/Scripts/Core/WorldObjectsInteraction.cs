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
        GameObject dialogueBox = GameObject.Find("VN controller/Root/Canvas - Main/LAYERS/4 - Dialogue");
        Transform cameraLocation = transform.Find("CameraLocation");
        dialogueBox.SetActive(true);
        CameraManager.instance.MoveCameraTo(cameraLocation);
        if(WorldManager.instance.currentGameEvent != null)
        StartConversation();
        WorldManager.instance.currentGameEvent.UpdateEvent();
        transform.parent.gameObject.SetActive(false);
    }


    void StartConversation()
    {
        List<string> lines;
        if(WorldManager.instance.currentGameEvent is Scene)
        {
            string currentSceneName = ((Scene)WorldManager.instance.currentGameEvent).name;
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
}
