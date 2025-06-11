using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;


[System.Serializable]
public class EventRoom
{
    public string name;
    public GameObject worldObjects;
}


[CreateAssetMenu(menuName = "Game Events/Free Roam Event")]
public class FreeRoamEvent : GameEvent
{
    public List<EventRoom> allowedRooms;

    protected void OnFinish()
    {
        base.OnFinish();
    }

    public override void CheckIfFinished()
    {
        if (isFinished)
            OnFinish();
    }

    public override void UpdateEvent()
    {
        if(startEventImmediately)
        {
            // checks the current room's objects (mostly characters) in this event, and loads the event's current room objects
            GameObject objects = (allowedRooms.Find(room => room.name == WorldManager.instance.currentRoom.name))
            ?.worldObjects;
            if (objects != null && GameObject.Find("World/World Objects/Characters") == null)
            WorldManager.instance.CreateCharacters(objects);
        }
        else
        {
            startEventImmediately = true;
        }
    }


    public override void PlayEvent()
    {
    }
}