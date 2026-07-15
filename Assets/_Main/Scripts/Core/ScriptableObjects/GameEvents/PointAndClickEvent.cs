using System.Collections.Generic;
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
        CameraManager.instance.MoveCameraTo(GameObject.Find("World/CameraStartPos").transform);
        
        base.OnFinish();
    }
}