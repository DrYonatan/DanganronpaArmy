using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Events/Point And Click Event")]
public class PointAndClickEvent : WorldEvent
{
    public List<string> requiredObjects;

    private bool AreAllClicked(Dictionary<string, ObjectData> datas)
    {
        bool finished = true;

        foreach (string requiredObject in requiredObjects)
        {
            if (!datas.TryGetValue(requiredObject, out var data))
                return true;

            if (!data.isClicked)
            {
                finished = false;
            }
        }

        return finished;
    }

    private bool IsInAnyDictionary()
    {
        foreach (string requiredObject in requiredObjects)
        {
            if (!charactersData.ContainsKey(requiredObject) && !objectsData.ContainsKey(requiredObject))
                return false;
        }

        return true;
    }

    public override void CheckIfFinished()
    {
        isFinished = AreAllClicked(charactersData) && AreAllClicked(objectsData) && IsInAnyDictionary();

        if (isFinished)
            OnFinish();
        else
        {
            OnNotFinished();
        }
    }

    protected override void OnFinish()
    {
        CameraManager.instance.StopAllPreviousOperations();
        WorldManager.instance.currentRoom.OnEventFinished();
        
        base.OnFinish();
    }

    public override bool CanExitRoom()
    {
        if (roomDatas.Find(room => room.room.roomName == WorldManager.instance.currentRoom.roomName).isExitable)
            return true;

        foreach (WorldObject obj in WorldManager.instance.currentRoomModel.GetComponentsInChildren<WorldObject>()
                     .ToList()
                )
        {
            if (requiredObjects.Contains(obj.id) && !obj.isClicked)
                return false;
        }

        foreach (string id in WorldManager.instance.currentRoomData.additionalObjectsToExit)
        {
            if (!objectsData.ContainsKey(id) || !objectsData[id].isClicked)
                return false;
        }

        return true;
    }
}