using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

public class WorldObjectsInteraction : MonoBehaviour
{
    public bool isClicked = false;
    public TextAsset text1;
    public TextAsset text2;
    // Start is called before the first frame update
    void Start()
    {

    }
    
    private void OnMouseDown()
    {
        if(!DialogueSystem.instance.isActive) 
        {
            GameObject dialogueBox = GameObject.Find("VN controller/Root/Canvas - Main/LAYERS/4 - Dialogue");
            Transform cameraLocation = transform.Find("CameraLocation");
            SoundManager.instance.PlaySoundEffect("click");
            StartCoroutine(MoveAndRotateCameraTo(cameraLocation));
            
        }
        
    }

    IEnumerator MoveAndRotateCameraTo(Transform transform)
    {
        float duration = 0.5f;
        yield return StartCoroutine(CameraManager.instance.RotateCameraTo(transform, duration));
        StartCoroutine(CameraManager.instance.MoveCameraTo(transform, duration));
        CameraManager.instance.setInitialPosition(transform);
        if(WorldManager.instance.currentGameEvent != null)
            StartConversation();
            WorldManager.instance.currentGameEvent.UpdateEvent();
    }

    void StartConversation()
    {
        List<string> lines;
        if(WorldManager.instance.currentGameEvent is PointAndClickEvent)
        {
            if (!isClicked)
            {
                lines = FileManager.ReadTextAsset(text1);
            }
            else
            {
                lines = FileManager.ReadTextAsset(text2 ? text2 : text1);
            }
            DialogueSystem.instance.Say(lines);
            isClicked = true;
        }

    }

}
