using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Room : ScriptableObject
{
    [Serializable]
    public class RoomEnvironment
    {
        public RoomModel prefab;
        public TimeOfDay timeOfDay;
    }
    
    public List<RoomEnvironment> roomVersions = new ();
    public string name;

    public Interactable currentInteractable;

    public bool isInside = false;

    public abstract void MovementControl();

    public virtual IEnumerator OnLoad()
    {
        CameraManager.instance?.ChangeCameraBackground(isInside);
        return null;
    }

    public virtual IEnumerator AppearAnimation()
    {
        yield return null;
    }

    public abstract void OnConversationEnd();

    public RoomModel GetTimeOfDayVersion(TimeOfDay timeOfDay)
    {
        RoomModel model = roomVersions.Find(x => x.timeOfDay == timeOfDay)?.prefab;
        if(model == null)
            return roomVersions[0].prefab;
        
        return model;
    }
}