using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DIALOGUE;

[System.Serializable]
public class NameAndEvent
{
    public string name;
    public GameEvent gameEvent;
}

[CreateAssetMenu(menuName = "Game Events/Free Roam Event/Investigation Event")]
public class InvestigationEvent : FreeRoamEvent
{
    [SerializeField] public List<NameAndEvent> assetEvents;

    public Dictionary<string, GameEvent>
        gameEvents = new Dictionary<string, GameEvent>(); // The string represents the room the event takes place in

    public override void UpdateEvent()
    {
        isFinished = true;

        foreach (GameEvent gameEvent in gameEvents.Values)
        {
            if (!gameEvent.isFinished)
                isFinished = false;
        }

        if (gameEvents.ContainsKey(WorldManager.instance.currentRoom.name) && !isFinished)
        {
            GameEvent gameEvent = gameEvents[WorldManager.instance.currentRoom.name];
            WorldManager.instance.currentGameEvent = gameEvent;
            gameEvent.PlayEvent();
        }
    }

    public override void CheckIfFinished()
    {
        if (isFinished)
            OnFinish();
        else
            DialogueSystem.instance.SetIsActive(false);
    }


    public override void PlayEvent()
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