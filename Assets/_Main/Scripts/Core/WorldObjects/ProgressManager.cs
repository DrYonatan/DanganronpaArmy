using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTERS;
using DIALOGUE;

public class ProgressManager : MonoBehaviour
{
    public static ProgressManager instance { get; private set; }
    public List<GameEvent> assetGameEvents;
    public Dictionary<string, GameEvent> runtimeGameEvents = new ();

    public CharactersFreeTimeEventsSO characterEventsAsset;
    public Dictionary<string, int> charactersRanks = new (); // Free time events ranks for each character

    private void Awake()
    {
        instance = this;
        // Creates the runtime events themselves without the condition events
        foreach(GameEvent gameEvent in assetGameEvents) 
        {
            GameEvent runTimeEvent = Instantiate(gameEvent);
            runTimeEvent.conditionEvents = new List<GameEvent>();
            runtimeGameEvents.Add(gameEvent.name, runTimeEvent);
        }
        

        foreach(var (key, value) in runtimeGameEvents)
        {
            foreach(GameEvent conditionEvent in GetAssetEventByName(key).conditionEvents)
            {
                value.conditionEvents.Add(runtimeGameEvents[conditionEvent.name]);
            }
        }

        foreach(CharacterFreeTimeEvents characterEvents in characterEventsAsset.charactersEvents)
        {
            charactersRanks.Add(characterEvents.characterName, 0);
        }


    }

    public void DecideWhichSceneToPlay()
    {
        foreach(GameEvent gameEvent in runtimeGameEvents.Values)
        {
            if(!gameEvent.isFinished)
            {
                if(gameEvent.CheckIfToPlay())
                {
                    WorldManager.instance.currentGameEvent = gameEvent;
                    gameEvent.PlayEvent();
                }
            }
        }

        
    }

    public GameEvent GetAssetEventByName(string name)
    {
        foreach(GameEvent gameEvent in assetGameEvents)
        {
            if(gameEvent.name.Equals(name))
            {
                return gameEvent;
            }
        }
        return null;
    }

    public PointAndClickEvent GetEventByName(string name)
    {
        foreach(GameEvent gameEvent in runtimeGameEvents.Values)
        {
            if(gameEvent is PointAndClickEvent)
            {
                if (((PointAndClickEvent)gameEvent).name == name)
                    return (PointAndClickEvent)gameEvent;
            }
            
        }
        return null;
    }
}
