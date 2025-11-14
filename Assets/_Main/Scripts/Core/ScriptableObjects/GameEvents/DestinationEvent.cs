using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

[CreateAssetMenu(menuName = "Game Events/Free Roam Event/Destination Event")]
public class DestinationEvent : FreeRoamEvent
{
    public string targetRoomName;

    public override void CheckIfFinished()
    {
        base.CheckIfFinished();
        
        if (WorldManager.instance.currentRoom.name.Equals(targetRoomName))
        {
            isFinished = true;
        }
    }


    public override void OnStart()
    {
    }
}