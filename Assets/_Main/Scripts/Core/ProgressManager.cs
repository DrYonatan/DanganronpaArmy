using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    public static ProgressManager instance { get; private set; }
    public List<GameEvent> gameEvents = new List<GameEvent>();
    private void Awake()
    {
        instance = this;
    }

    public void DecideWhichSceneToPlay()
    {
        foreach(GameEvent gameEvent in gameEvents)
        {
            if(!gameEvent.CheckIfFinished())
            {
                
                if(gameEvent.CheckIfToPlay())
                {
                    WorldManager.instance.currentGameEvent = gameEvent;
                    gameEvent.PlayEvent();
                }
            }
        }

        
    }

    public Scene GetEventByName(string name)
    {
        foreach(GameEvent gameEvent in gameEvents)
        {
            if(gameEvent is Scene)
            {
                if (((Scene)gameEvent).name == name)
                    return (Scene)gameEvent;
            }
            
        }
        return null;
    }
}
