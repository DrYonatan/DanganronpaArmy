using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

[CreateAssetMenu(menuName ="Game Events/Investigation Event")]
public class InvestigationEvent : GameEvent 
{
    public Dictionary<string, GameEvent> gameEvents;

    public override void UpdateEvent()
    {
        GameEvent gameEvent = gameEvents[WorldManager.instance.currentRoom.name];
        if (gameEvent != null) 
        gameEvent.PlayEvent();
    }

    public override void CheckIfFinished()
    {
        if(isFinished)
        OnFinish();
        else
        DialogueSystem.instance.SetIsActive(false);
    }


    public override void PlayEvent()
    {
       
        
    }

    public override void OnFinish()
    {

    }

}
