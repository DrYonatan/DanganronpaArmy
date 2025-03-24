using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Game Events/Free Roam Event")]
public class FreeRoamEvent : GameEvent
{
    public GameObject characterPrefab;
    public string currentRoomName;
    public string targetRoomName;

    public override void CheckIfFinished()
    {
        if(isFinished)
        {
            OnFinish();
        }
        
    }

    public override void UpdateEvent()
    {
        if(currentRoomName.Equals(targetRoomName))
        {
            isFinished = true;
        }
    }


    public override void PlayEvent()
    {

    }

    public override void OnFinish()
    {

    }
}
