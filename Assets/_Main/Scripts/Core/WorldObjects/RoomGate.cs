using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;
using System.Linq;


public class RoomGate : Interactable
{
    public Room roomToLoad;

    public async override void Interact()
    {
        base.Interact();
        if (((FreeRoamEvent)(WorldManager.instance.currentGameEvent)).allowedRooms.Any(item =>
                item.name == roomToLoad.name) ||
            ((FreeRoamEvent)(WorldManager.instance.currentGameEvent)).allowedRooms.Count == 0)
        {
            await WorldManager.instance.LoadRoom(roomToLoad);
            WorldManager.instance.currentGameEvent.UpdateEvent();
            WorldManager.instance.currentGameEvent.CheckIfFinished();
        }
        else
        {
            // if the room is unallowed, read the "you can't go into this room" text
            DialogueSystem.instance.Say(
                FileManager.ReadTextAsset(WorldManager.instance.currentGameEvent.unallowedText));
        }
    }
}