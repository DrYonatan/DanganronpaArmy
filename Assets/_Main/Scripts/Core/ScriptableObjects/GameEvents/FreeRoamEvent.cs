using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

[CreateAssetMenu(menuName ="Game Events/Free Roam Event")]
public class FreeRoamEvent : GameEvent
{
    public GameObject characterPrefab;
    public string targetRoomName;
    public List<string> allowedRooms;
    public TextAsset unallowedText;
    public TextAsset finishText;

    public override void CheckIfFinished()
    {
        if(isFinished)
        {
            OnFinish();
        }
        
    }

    public override void UpdateEvent()
    {
        if(WorldManager.instance.currentRoom.name.Equals(targetRoomName))
        {
            isFinished = true;
        }
    }


    public override void PlayEvent()
    {
        //GameObject.Find("VN controller/Root/Canvas - Main/LAYERS/4 - Dialogue").GetComponent<CanvasGroup>().alpha = 0;
    }

    public override void OnFinish()
    {
        List<string> lines = FileManager.ReadTextAsset(finishText);
        if (lines != null)
            DialogueSystem.instance.Say(lines);
            
        ProgressManager.instance.DecideWhichSceneToPlay();
    }
}
