using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public abstract class Room : ScriptableObject
{
    [Serializable]
    public class RoomEnvironment
    {
        public RoomModel prefab;
        public TimeOfDay timeOfDay;
    }

    public List<RoomEnvironment> roomVersions = new();
    public string roomName;

    public Interactable currentInteractable;

    public bool isInside = false;

    public abstract void MovementControl();

    public virtual IEnumerator OnLoad()
    {
        CameraManager.instance?.ChangeCameraBackground(isInside);
        return null;
    }

    public void SetInteractables(List<EventAdditionalObjectData> additionalObjectDatas)
    {
        RoomModel roomModel = WorldManager.instance.currentRoomModel;
        RoomModel originalModel =
            roomVersions.Find(x => x.timeOfDay == WorldManager.instance.currentTime).prefab;


        foreach (ConversationInteractable interactable in roomModel.interactables)
        {
            EventAdditionalObjectData data = additionalObjectDatas.Find(x => x.id == interactable.id);
            if (data != null)
            {
                interactable.texts = new List<VNConversationSegment>(data.texts);
            }

            else
            {
                ConversationInteractable originalInteractable =
                    originalModel.interactables.Find(x => x.id == interactable.id);
                interactable.texts = originalInteractable.texts;
                interactable.clickCount = originalInteractable.clickCount;
                interactable.isClicked = originalInteractable.isClicked;
            }
        }
    }

    public virtual IEnumerator AppearAnimation()
    {
        yield return null;
    }

    public abstract void OnConversationEnd();

    public RoomModel GetTimeOfDayVersion(TimeOfDay timeOfDay)
    {
        RoomModel model = roomVersions.Find(x => x.timeOfDay == timeOfDay)?.prefab;
        if (model == null)
            return roomVersions[0].prefab;

        return model;
    }
}