using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

[CreateAssetMenu(menuName = "Game Events/Free Roam Event/Destination Event")]
public class DestinationEvent : FreeRoamEvent
{
    public GameObject characterPrefab;
    public string targetRoomName;

    public override void UpdateEvent()
    {
        base.UpdateEvent();

        if (WorldManager.instance.currentRoom.name.Equals(targetRoomName))
        {
            isFinished = true;
        }
    }


    public override void PlayEvent()
    {
    }
}