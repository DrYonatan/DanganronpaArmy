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
public abstract class FreeRoamEvent : GameEvent
{
    public List<EventRoom> allowedRooms;
}
