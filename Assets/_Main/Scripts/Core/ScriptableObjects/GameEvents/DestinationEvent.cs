using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

[CreateAssetMenu(menuName = "Game Events/Free Roam Event/Destination Event")]
public class DestinationEvent : FreeRoamEvent
{
    public GameObject characterPrefab;
    public string targetRoomName;

    public override void CheckIfFinished()
    {
        if(isFinished)
        {
            OnFinish();
        }
        
    }

    public override void UpdateEvent()
    {
        // checks the current room's objects (mostly characters) in this event, and loads the event's current room objects
        GameObject objects = (allowedRooms.Find(room => room.name == WorldManager.instance.currentRoom.name))?.worldObjects;
        if(objects != null && GameObject.Find("World/World Objects/Characters") == null)
        WorldManager.instance.CreateCharacters(objects);

        if(WorldManager.instance.currentRoom.name.Equals(targetRoomName))
        {
            isFinished = true;
        }
    }


    public override void PlayEvent()
    {

    }

    public override void OnFinish()
    {
        base.OnFinish();
    }
}
