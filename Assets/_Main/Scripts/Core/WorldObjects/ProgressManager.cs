using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTERS;
using DIALOGUE;

public class ProgressManager : MonoBehaviour
{
    public static ProgressManager instance { get; private set; }
    public List<GameEvent> gameEvents;

    public CharactersFreeTimeEventsSO characterEventsAsset;
    public Dictionary<string, int> charactersRanks = new (); // Free time events ranks for each character

    public GameEvent currentGameEvent;
    public int currentGameEventIndex;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        currentGameEvent = Instantiate(currentGameEvent);
        currentGameEvent.OnStart();
    }

    public void OnEventFinished()
    {
        currentGameEventIndex++;
        currentGameEvent = Instantiate(gameEvents[currentGameEventIndex]);
        currentGameEvent.OnStart();
    }

    // public void DecideWhichSceneToPlay()
    // {
    //     foreach(GameEvent gameEvent in runtimeGameEvents.Values)
    //     {
    //         if(!gameEvent.isFinished)
    //         {
    //             WorldManager.instance.currentGameEvent = gameEvent;
    //             gameEvent.PlayEvent();
    //         }
    //     }
    // }

        
    }

    // public GameEvent GetAssetEventByName(string name)
    // {
    //     foreach(GameEvent gameEvent in assetGameEvents)
    //     {
    //         if(gameEvent.name.Equals(name))
    //         {
    //             return gameEvent;
    //         }
    //     }
    //     return null;
    // }
    //
    // public PointAndClickEvent GetEventByName(string name)
    // {
    //     foreach(GameEvent gameEvent in runtimeGameEvents.Values)
    //     {
    //         if(gameEvent is PointAndClickEvent)
    //         {
    //             if (((PointAndClickEvent)gameEvent).name == name)
    //                 return (PointAndClickEvent)gameEvent;
    //         }
    //         
    //     }
    //     return null;
    // }
