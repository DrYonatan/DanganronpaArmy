using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

[CreateAssetMenu(menuName = "Game Events/Free Roam Event")]
public class FreeRoamEvent : GameEvent
{
    protected void OnFinish()
    {
        base.OnFinish();
    }

    public override void CheckIfFinished()
    {
        if(startEventImmediately)
        {
            // checks the current room's objects (mostly characters) in this event, and loads the event's current room objects
            GameObject objects = (roomDatas.Find(room => room.room.name == WorldManager.instance.currentRoom.name))
                ?.worldObjects;
            if (objects != null && GameObject.Find("World/World Objects/Characters") == null)
                WorldManager.instance.CreateCharacters(objects);
        }
        else
        {
            startEventImmediately = true;
        }
        
        if (isFinished)
            OnFinish();
    }

    public override void OnStart()
    {
    }
}