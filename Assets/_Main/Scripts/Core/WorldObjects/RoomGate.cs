using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

public class RoomGate : MonoBehaviour
{
    public Room roomToLoad;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        if(((FreeRoamEvent)(WorldManager.instance.currentGameEvent)).allowedRooms.Contains(roomToLoad.name) || 
        ((FreeRoamEvent)(WorldManager.instance.currentGameEvent)).allowedRooms[0].ToLower() == "all")
        {
            WorldManager.instance.LoadRoom(roomToLoad);
            WorldManager.instance.currentGameEvent.UpdateEvent();
            WorldManager.instance.currentGameEvent.CheckIfFinished();
        }
        else
        {
            // if the room is unallowed, read the "you can't go into this room" text
            DialogueSystem.instance.Say(FileManager.ReadTextAsset(((FreeRoamEvent)WorldManager.instance.currentGameEvent).unallowedText));
        }
    }
}
