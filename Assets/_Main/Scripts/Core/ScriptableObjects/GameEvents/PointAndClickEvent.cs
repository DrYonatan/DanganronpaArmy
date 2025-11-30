using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Events/Point And Click Event")]
public class PointAndClickEvent : GameEvent
{
    private bool AreAllClicked(Dictionary<string, ObjectData>.ValueCollection datas)
    {
        bool finished = true;

        foreach (ObjectData data in datas)
        {
            if (!data.isClicked)
                finished = false;
        }

        return finished;
    }

    public override void CheckIfFinished()
    {
        bool allCharactersClicked = AreAllClicked(charactersData.Values);

        bool allObjectsClicked = AreAllClicked(objectsData.Values);

        isFinished = allCharactersClicked && allObjectsClicked;

        if (isFinished)
            OnFinish();
    }

    protected override void OnFinish()
    {
        CameraManager.instance.MoveCameraTo(GameObject.Find("World/CameraStartPos").transform);
        
        base.OnFinish();
    }
}