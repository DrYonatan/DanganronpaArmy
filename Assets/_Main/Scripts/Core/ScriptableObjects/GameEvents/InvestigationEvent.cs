using System.Collections.Generic;
using UnityEngine;
using System;
using DIALOGUE;
using UnityEngine.Serialization;

[Serializable]
public class NameAndEvent
{
    public string name;
    public WorldEvent gameEvent;
}

[CreateAssetMenu(menuName = "Game Events/Free Roam Event/Investigation Event")]
public class InvestigationEvent : FreeRoamEvent
{
    [SerializeField] public List<NameAndEvent> assetEvents;

    public Dictionary<string, WorldEvent>
        gameEvents = new Dictionary<string, WorldEvent>(); // The string represents the room the event takes place in

    public override void CheckIfFinished()
    {
        isFinished = true;

        foreach (WorldEvent gameEvent in gameEvents.Values)
        {
            if (!gameEvent.isFinished)
                isFinished = false;
        }

        if (gameEvents.ContainsKey(WorldManager.instance.currentRoom.roomName) && !isFinished)
        {
            WorldEvent worldEvent = gameEvents[WorldManager.instance.currentRoom.roomName];
            ProgressManager.instance.currentGameEvent = worldEvent;
            worldEvent.OnStart();
        }
        
        if (isFinished)
            OnFinish();
        else
            DialogueSystem.instance.SetIsActive(false);
    }


    public override void OnStart()
    {
    }

    private void OnFinish()
    {
        base.OnFinish();
    }

    void Awake()
    {
        foreach (NameAndEvent gameEvent in assetEvents)
        {
            gameEvents.Add(gameEvent.name, Instantiate(gameEvent.gameEvent));
        }
    }
}